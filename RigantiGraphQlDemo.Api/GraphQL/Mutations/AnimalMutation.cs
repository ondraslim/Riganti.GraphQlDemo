using GraphQL;
using GraphQL.Types;
using RigantiGraphQlDemo.Api.GraphQL.InputTypes;
using RigantiGraphQlDemo.Api.GraphQL.Types;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Linq;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
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
}