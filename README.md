# GraphQL ASP.NET Core demo (2 - Mutations)

###### Updating data via GraphQL

To update data via *GraphQL* we use so called *mutations*, which represent standard `POST`, `PUT` and `DELETE` operations of *REST* APIs.

###### User Data Input
Lets say we want to have CRUD operations available on our `Animal` class. To create or update an animal, we would need a user input with new data. For this purpose, we can extend `InputObjectGraphType` class and specify, what is expected as a input using the following block of code:

```csharp
public class AnimalInputType : InputObjectGraphType
{
    public AnimalInputType()
    {
        Field<NonNullGraphType<StringGraphType>>(nameof(Animal.Name));
        Field<StringGraphType>(nameof(Animal.Species));
        Field<IdGraphType>(nameof(Animal.FarmId));
    }
}
```

Using `AnimalInputType` we can also provide a validation check so the user cannot insert non-senses.


###### Update data
To perform a mutation you need to have a root Mutation object that is an `ObjectGraphType`. Mutations make modifications to data and return a result.
For creation and update of an animal, we need to accept our input type `AnimalInputType` as a argument, for deletion, all we need is an ID of animal. Create a class `AnimalMutation` which will handle mutations of animals:

```csharp
public class AnimalMutation : ObjectGraphType
{
    public AnimalMutation(AnimalFarmDbContext dbContext)
    {
        Field<AnimalType>(
            "addAnimal",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<AnimalInputType>> {Name = "newAnimal"}),
            resolve: context =>
            {
                var animal = context.GetArgument<Animal>("newAnimal");
                var entry = dbContext.Add(animal);
                dbContext.SaveChanges();

                return entry.Entity;
            }
        );

        Field<AnimalType>(
            "updateAnimal",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "id"},
                new QueryArgument<NonNullGraphType<AnimalInputType>> {Name = "updateAnimal"}
            ),
            resolve: context =>
            {
                var animal = context.GetArgument<Animal>("updateAnimal");
                var animalId = context.GetArgument<int>("id");

                var oldAnimal = dbContext.Animals.FirstOrDefault(a => a.Id == animalId);
                if (oldAnimal == default)
                {
                    context.Errors.Add(new ExecutionError($"Couldn't find any animal of id '{animalId}'."));
                    return null;
                }

                oldAnimal.Name = animal.Name;
                oldAnimal.Species = animal.Species;
                oldAnimal.FarmId = animal.FarmId;

                dbContext.SaveChanges();

                return oldAnimal;
            }
        );

        Field<AnimalType>(
            "deleteAnimal",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "animalId"}),
            resolve: context =>
            {
                var animalId = context.GetArgument<int>("animalId");
                var animal = dbContext.Animals.FirstOrDefault(a => a.Id == animalId);
                if (animal == default)
                {
                    context.Errors.Add(new ExecutionError($"Couldn't find any animal of id '{animalId}'."));
                    return null;
                }

                dbContext.Animals.Remove(animal);
                dbContext.SaveChanges();

                return animal;
            }
        );
    }
}
```

For errors, we can use `ExecutionError` to describe, what's wrong, e.g. from previous example:

```csharp
if (animal == default)
{
    context.Errors.Add(new ExecutionError($"Couldn't find any animal of id '{animalId}'."));
    return null;
}             
```


###### Register new mutations

To provide our newly created mutations to the GraphQL app, we simply just add the mutation in our `AppSchema`, which can have 1 root *Query* and 1 root *Mutation*:

```csharp
public class AppSchema : global::GraphQL.Types.Schema
{
    public AppSchema(IDependencyResolver resolver) : base(resolver)
    {
        Query = resolver.Resolve<AppQuery>();
        Mutation = resolver.Resolve<AnimalMutation>();
    }
}
```


##### GraphQL mutations demo

In queries we use `mutation` instead of `query` keyword. A useful feature of GraphQL requests are *query variables*, they are basically a parameter to our mutation request:

```
mutation UpdateAnimal($animalUpdate: AnimalInputType!) {
  updateAnimal(id: 1, updateAnimal: $animalUpdate) {
    id, name, species
  }
}
```

Having a query variable animalUpdate:

```
{  
  "animalUpdate": {
    "name": "animal updated name",
    "species": "animal updated species",
    "farmId": 2
  }
}
```

**Sources:**

* https://graphql-dotnet.github.io/docs/getting-started/introduction
* https://graphql.org/learn/queries/
