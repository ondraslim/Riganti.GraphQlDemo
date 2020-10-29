using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public interface IAnimalConnection
    {
        IReadOnlyList<IAnimal> Nodes { get; }
    }
}
