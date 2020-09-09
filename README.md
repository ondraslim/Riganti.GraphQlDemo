# GraphQL ASP.NET Core demo (1 - Basics)


#### GraphQL Middleware

######
* **GraphQL** - adds *GraphQL* superpowers for the app
* **GraphiQL** - adds in-browser IDE for exploring *GraphQL* (syntax/error highlighting, query auto-completion...)

Add *GraphiQL* to the app and specify its endpoint using the following line to the `Configure(...)` method in your `Startup.cs`:

```csharp
app.UseGraphiQl("/GraphQL");
```


##### GraphQL Types
*GraphQL* is not boundto any language nor framework therefore it does not understand CLR classes. Thus we have to explicitly create *GraphQL types* and specify their *fields* by extending `ObjectGraphType<T>` class. In our case, we can specify *AnimalType* using the following block of code:

```csharp  
public class AnimalType : ObjectGraphType<Animal>
{
    public AnimalType()
   {
       Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Animal.");
       Field(x => x.Name).Description("The name of the Animal.");
       Field(x => x.Species).Description("The species of the Animal.");
       Field(x => x.FarmId).Description("The id of farm where the Animal lives.");
       Field(x => x.Farm, type: typeof(ListGraphType<FarmType>)).Description("The farm where the Animal lives.");
   }
}
```

This way we can also hide some properties, e.g. property `SecretPiggyBankLocation` of class `Person` by simply not adding them to the `PersonType` class:

```csharp
public class PersonType : ObjectGraphType<Person>
{
    public PersonType()
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
        Field(x => x.Name).Description("The name of the Farm.");
        Field(x => x.Farms, type: typeof(ListGraphType<FarmType>)).Description("The farms of the Person.");

        // We do not want to expose SecretPiggyBankLocation
        //Field(x => x.SecretPiggyBankLocation).Description("The secret location of person's piggy bank. (should not be available!)");
    }
}
```

##### GraphQL Query
After specifying all the *GraphQL types*, we need to add a *GraphQL Query* which will handle fetching the requested data and expose "endpoints" of our API. In our example, we want to expose only a list of farms and persons and getting person by ID. To do this, create class `AppQuery.cs` with the following content:

```csharp
public class AppQuery : ObjectGraphType
{
    public AppQuery(AnimalFarmDbContext dbContext)
    {
        Field<PersonType>(
            "Person",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> {Name = "id", Description = "The ID of the Person."}),
            resolve: context =>
            {
                var id = context.GetArgument<int>("id");
                return dbContext.Persons.Include(p => p.Farms).ThenInclude(pf => pf.Animals).FirstOrDefault(p => p.Id == id);
            }
        );

        Field<ListGraphType<PersonType>>(
            "Persons",
            resolve: context =>
            {
                return dbContext.Persons.Include(p => p.Farms).ThenInclude(pf => pf.Animals);
            }
        );

        Field<ListGraphType<FarmType>>(
            "Farms",
            resolve: context =>
            {
                return dbContext.Farms.Include(f => f.Animals).Include(f => f.Person);
            }
        );
    }
}
```


##### GraphQL Client Request Model

The client will always make a *HTTP POST* request containing query, query name, operation name and variables. We can create a class which we can use as a model for such request, hence the class `GraphQlQuery.cs`:

```csharp
public class GraphQlQuery
{
    public string OperationName { get; set; }
    public string NamedQuery { get; set; }
    public string Query { get; set; }
    public JObject Variables { get; set; }
}
```


##### GraphQL Schema

Then we should define *GraphQL schema* class and register the specified query `AppQuery.cs`:

```csharp
public class AppSchema : global::GraphQL.Types.Schema
{
    public AppSchema(IDependencyResolver resolver) : base(resolver)
    {
        Query = resolver.Resolve<AppQuery>();
    }
}
```

This will make our query known and available to GrapQL. The constructor of `AppSchema` accepts *DependencyResolver* which we register using `ConfigureServices` in `Startup.cs`:

```csharp
services.AddScoped<IDependencyResolver>(_ => new FuncDependencyResolver(_.GetRequiredService));
```


##### GraphQL Endpoint
In the last step of basic *GraphQL* setup, we prepare a *GraphQL controller* which will take care of all *GraphQL* requests from the clients. The controller will have only one endpoint accepting the *QraphQL query model* which we specified in previous steps.

The controller needs a *Schema* (to know the options) and *DocumentExecuter* (to execute requested query).

```csharp
[Route("[controller]")]
public class GraphQlController : Controller
{
    private readonly ISchema schema;
    private readonly IDocumentExecuter documentExecutor;

    public GraphQlController(ISchema schema, IDocumentExecuter documentExecutor)
    {
        this.schema = schema;
        this.documentExecutor = documentExecutor;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GraphQlQuery query)
    {
        var inputs = query.Variables.ToInputs();
        var executionOptions = new ExecutionOptions
        {
            Schema = schema,
            Query = query.Query,
            OperationName = query.OperationName,
            Inputs = inputs
        };

        var result = await documentExecutor.ExecuteAsync(executionOptions);

        if (result.Errors?.Count > 0)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}
```

Then we need to register all of the necessary classes in our IoC container. We can use `ConfigureServices()` method in `Startup.cs` to achieve it, simply add the following lines:

```csharp
services.AddScoped<ISchema, AppSchema>();
services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
services
    .AddGraphQL(o => { o.ExposeExceptions = false; })
    .AddGraphTypes(ServiceLifetime.Scoped);
```


##### GraphQL demo
With this setup, we should be able to simply just run the application and test our GraphQL demo in the /GraphQL endpoint. In *GraphiQL* IDE we can experiment with queries to fetch data, some of the sample queries:

```
query DetailedList {
  persons {
    id
    name
    farms {
      id
      name
      animals {
        id
        name
        species
      }
    }
  }
}

query PersonDetail {
  person(id: 2) {
    name
    farms {
      name
    }
  }
}

query Farms {
  farms {
    name
    animals {
      id
      name
    }
  }
}
```

A sample server response to query PersonDetail looks like this:

```json
{
  "data": {
    "person": {
      "name": "Mr. Whymper",
      "farms": [
        {
          "name": "AnimalFarm"
        }
      ]
    }
  }
}
```
