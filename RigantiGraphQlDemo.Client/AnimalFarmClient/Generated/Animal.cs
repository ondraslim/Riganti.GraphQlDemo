using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public class Animal
        : IAnimal
    {
        public Animal(
            string id, 
            string name, 
            string species, 
            string farmId)
        {
            Id = id;
            Name = name;
            Species = species;
            FarmId = farmId;
        }

        public string Id { get; }

        public string Name { get; }

        public string Species { get; }

        public string FarmId { get; }
    }
}
