using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public class AnimalList
        : IAnimalList
    {
        public AnimalList(
            IAnimalConnection animals)
        {
            Animals = animals;
        }

        public IAnimalConnection Animals { get; }
    }
}
