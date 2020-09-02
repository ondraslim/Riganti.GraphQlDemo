using GraphQL;
using GraphQL.Types;
using RigantiGraphQlDemo.Api.GraphQL.InputTypes;
using RigantiGraphQlDemo.Api.GraphQL.Types;
using RigantiGraphQlDemo.Dal.DataStore;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
    public class AnimalMutation : ObjectGraphType
    {
        public AnimalMutation(IDataStore dataStore)
        {
            Field<AnimalType>()
                .Name("addAnimal")
                .Argument<NonNullGraphType<AnimalInputType>>("newAnimal", "new animal data")
                .ResolveAsync(async context =>
                {
                    var animal = context.GetArgument<Animal>("newAnimal");
                    return await dataStore.CreateAnimalAsync(animal);
                });

            Field<AnimalType>()
                .Name("updateAnimal")
                .Argument<NonNullGraphType<IdGraphType>>("id", "id of Animal to update")
                .Argument<NonNullGraphType<AnimalInputType>>("updateAnimal", "new animal data")
                .ResolveAsync(async context =>
                {
                    var animal = context.GetArgument<Animal>("updateAnimal");
                    var animalId = context.GetArgument<int>("id");
                    var updatedAnimal = await dataStore.UpdateAnimalAsync(animalId, animal);
                    if (updatedAnimal == null)
                        context.Errors.Add(new ExecutionError($"Couldn't find any animal of id '{animalId}'."));
                    return updatedAnimal;
                });

            Field<AnimalType>()
                .Name("deleteAnimal")
                .Argument<NonNullGraphType<IdGraphType>>("animalId", "id of Animal to remove")
                .ResolveAsync(async context =>
                {
                    var animalId = context.GetArgument<int>("animalId");
                    var updatedAnimal = await dataStore.DeleteAnimalAsync(animalId);
                    if (updatedAnimal == null)
                        context.Errors.Add(new ExecutionError($"Couldn't find any animal of id '{animalId}'."));
                    return updatedAnimal;
                });
        }
    }
}