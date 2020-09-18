using GraphQL;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;
using RigantiGraphQlDemo.Api.Configuration;
using RigantiGraphQlDemo.Api.GraphQL.InputTypes;
using RigantiGraphQlDemo.Api.GraphQL.Types.AnimalTypes;
using RigantiGraphQlDemo.Dal.DataStore.Animal;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
    public class AnimalMutation : ObjectGraphType, IMutation
    {
        public AnimalMutation(IAnimalDataStore animalDataStore)
        {
            // authorize all mutations
            this.AuthorizeWith(Policies.LoggedIn);

            Field<AnimalType>()
                .Name("addAnimal")
                .Argument<NonNullGraphType<AnimalInputType>>("newAnimal", "new animal data")
                .ResolveAsync(async context =>
                {
                    var animal = context.GetArgument<Animal>("newAnimal");
                    return await animalDataStore.CreateAnimalAsync(animal);
                });

            Field<AnimalType>()
                .Name("updateAnimal")
                .Argument<NonNullGraphType<IdGraphType>>("id", "id of Animal to update")
                .Argument<NonNullGraphType<AnimalInputType>>("updateAnimal", "new animal data")
                .ResolveAsync(async context =>
                {
                    var animal = context.GetArgument<Animal>("updateAnimal");
                    var animalId = context.GetArgument<int>("id");
                    var updatedAnimal = await animalDataStore.UpdateAnimalAsync(animalId, animal);
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
                    var updatedAnimal = await animalDataStore.DeleteAnimalAsync(animalId);
                    if (updatedAnimal == null)
                        context.Errors.Add(new ExecutionError($"Couldn't find any animal of id '{animalId}'."));
                    return updatedAnimal;
                });
        }
    }
}