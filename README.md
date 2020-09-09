# GraphQL ASP.NET Core demo (3 - DataLoaders)

###### Minor project structure changes

For the sake of clarity we created class `DataStore` and its interface `IDataStore` which handles all communication with database. The `DataStore` is registered in our IoC container and used in our `AppQuery` and `AnimalMutation`. Also our types definition style changed while the logic remained the same.


###### Custom field resolver N+1 problem
So far we used default resolver for our GraphQL reference types. We might want to have a custom resolver for some of the reference types to add custom logic (e.g. we might want to filter out soft-deleted *Farms* of a *Person* in his *Farms* or any other extra logic).

To add custom resolve logic, we can add `ResolveAsync()` for *Persons Farms*:

```csharp
public class PersonType : ObjectGraphType<Person>
{
    public PersonType(IDataStore dataStore)
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
        Field(x => x.Name).Description("The name of the Farm.");
        Field<ListGraphType<FarmType>, IEnumerable<Farm>>()
            .Name("Farms")
            .Description("The farms of the Person.")
            .ResolveAsync(async ctx =>
                // logic can be added here
                await dataStore.GetFarmsByPersonIdAsync(ctx.Source.Id)
                // logic can be added here
            );

        // We do not want to expose SecretPiggyBankLocation
        //Field(x => x.SecretPiggyBankLocation).Description("The secret location of person's piggy bank. (should not be available!)");
    }
}
```

However after running a query to obtain persons and their farms:

```
query CustomerResolverTest {
	persons {
		id, 
		name,
		farms {
		  name
		}
	},
 }
```

We can see in logs that for each person a new DB request for his farms was made:


```
Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (31ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT "p"."Id", "p"."Name", "p"."SecretPiggyBankLocation"
FROM "Persons" AS "p"
Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (9ms) [Parameters=[@__personId_0='?'], CommandType='Text', CommandTimeout='30']
SELECT "f"."Id", "f"."Name", "f"."PersonId"
FROM "Farms" AS "f"
WHERE @__personId_0 = "f"."PersonId"
Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (3ms) [Parameters=[@__personId_0='?'], CommandType='Text', CommandTimeout='30']
SELECT "f"."Id", "f"."Name", "f"."PersonId"
FROM "Farms" AS "f"
WHERE @__personId_0 = "f"."PersonId"
```

We have only 2 persons stored each one having his own farm, but 3 DB requests were made, therefore we have a `N + 1` problem.


###### N+1 solution

To overcome this problem, we can use a `DataLoader`. They help in two ways:
1. Similar operations are batched together. This can make fetching data over a network much more efficient.
2. Fetched values are cached so if they are requested again, the cached value is returned.

###### DataLoader - Middleware

Lets create our own GraphQL middleware class `GraphQlMiddleware`, which will add a `DataLoaderDocumentListener` to the query exection options:

```csharp
public class GraphQlMiddleware
{
    private readonly RequestDelegate next;
    private readonly IDocumentWriter writer;
    private readonly IDocumentExecuter executor;

    public GraphQlMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor)
    {
        this.next = next;
        this.writer = writer;
        this.executor = executor;
    }

    public async Task InvokeAsync(HttpContext httpContext, ISchema schema, IServiceProvider serviceProvider)
    {
        if (httpContext.Request.Path.StartsWithSegments("/graphql") && 
            string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
        {
            using var streamReader = new StreamReader(httpContext.Request.Body);
            var body = await streamReader.ReadToEndAsync();

            //GraphQLRequest
            var request = JsonConvert.DeserializeObject<GraphQlQuery>(body);

            var result = await executor.ExecuteAsync(doc =>
            {
                doc.Schema = schema;
                doc.Query = request.Query;
                doc.OperationName = request.OperationName;
                doc.Inputs = request.Variables.ToInputs();
                doc.Listeners.Add(serviceProvider.GetRequiredService<DataLoaderDocumentListener>());
            }).ConfigureAwait(false);

            var json = writer.Write(result);
            await httpContext.Response.WriteAsync(json);
        }
        else
        {
            await next(httpContext);
        }
    }
}
```

We need to register `IDataLoaderContextAccessor` and `DataLoaderDocumentListener` in our IoC container:

```csharp
services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
services.AddSingleton<DataLoaderDocumentListener>();
```

###### DataLoader - usage
We will need to add new methods to our `IDataStore` which will help the `DataLoader` to optimize the queries execuction:

```csharp
Task<ILookup<int, Farm>> GetFarmsByPersonIdDataLoaderAsync(IEnumerable<int> personIds, CancellationToken token);
Task<IDictionary<int, Person>> GetPersonsByIdDataLoaderAsync(IEnumerable<int> personIds, CancellationToken token);
```

Having the Listener prepared, we can use them in our `PersonType` class:

```csharp
public class PersonType : ObjectGraphType<Person>
{
    public PersonType(IDataStore dataStore, IDataLoaderContextAccessor accessor)
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
        Field(x => x.Name).Description("The name of the Farm.");
        Field<ListGraphType<FarmType>, IEnumerable<Farm>>()
            .Name("Farms")
            .Description("The farms of the Person.")
            .ResolveAsync(ctx =>
              {
                   var farmLoader =
                       accessor.Context.GetOrAddCollectionBatchLoader<int, Farm>(
                           "GetFarmsByPersonId",
                           dataStore.GetFarmsByPersonIdDataLoaderAsync);

                   return farmLoader.LoadAsync(ctx.Source.Id);
               }
              );

        // We do not want to expose SecretPiggyBankLocation
        //Field(x => x.SecretPiggyBankLocation).Description("The secret location of person's piggy bank. (should not be available!)");
    }
}
```

And similarly in our `FarmType` class:

```csharp
public class FarmType : ObjectGraphType<Farm>
    {
        public FarmType(IDataStore dataStore, IDataLoaderContextAccessor accessor)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
            Field(x => x.Name).Description("The name of the Farm.");
            Field(x => x.PersonId).Description("The id of the Farm's owner.");
            Field<PersonType, Person>()
                .Name("Person")
                .Description("Farm's owner.")
                .ResolveAsync(ctx =>
                 {
                     var personLoader = accessor.Context.GetOrAddBatchLoader<int, Person>(
                         "GetPersonById",
                         dataStore.GetPersonsByIdDataLoaderAsync);
                     return personLoader.LoadAsync(ctx.Source.PersonId);
                 });

            Field(x => x.Animals, type: typeof(ListGraphType<AnimalType>)).Description("Farm's animals.");
        }
    } 
```

>Idea behind `GetOrAddBatchLoader` is that it waits until all the person IDs are queued. Then it fires of the `GetPersonsByIdDataLoaderAsync` method only when all the IDs are collected. Once the dictionary of Persons is returned with the passed in IDs; a person that belongs to a particular farm is returned from the field with some internal object mapping. This technique of queueing up IDs is called batching.


`GetOrAddCollectionBatchLoader` and `GetOrAddBatchLoader` both caches the values of the field for the lifetime of a GraphQl query. If you only want to use the caching feature and ignore batching, you can simply use the `GetOrAddLoader`.

> `DataLoaders` can also be chained together, see examples: https://graphql-dotnet.github.io/docs/guides/dataloader/#examples

> Notice, in our `PersonType` for `Farms`, we work with `ILookup` data structure instead of a dictionary. The only difference between them is ILookup can have multiple values against a single key whereas for the dictionary; a single key belongs to a single value.


##### GraphQL DataLoaders demo

Now using the same query as before, which resulted in N+1 problem:

```
query CustomerResolverTest {
	persons {
		id, 
		name,
		farms {
		  name
		}
	}
 }
```

The `DataLoader` will only request *Persons* and *Farms* once (batched together):
```
SELECT "p"."Id", "p"."Name", "p"."SecretPiggyBankLocation"
FROM "Persons" AS "p"
Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT "f"."Id", "f"."Name", "f"."PersonId"
FROM "Farms" AS "f"
WHERE "f"."PersonId" IN (1, 2)
```

###### Sources:

* https://graphql-dotnet.github.io/docs/guides/dataloader/
* https://github.com/fiyazbinhasan/GraphQLCore/tree/Part_X_DataLoader
