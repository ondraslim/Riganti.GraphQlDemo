using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public class AnimalConnection
        : IAnimalConnection
    {
        public AnimalConnection(
            IReadOnlyList<IAnimal> nodes)
        {
            Nodes = nodes;
        }

        public IReadOnlyList<IAnimal> Nodes { get; }
    }
}
