using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public interface IAnimal
    {
        string Id { get; }

        string Name { get; }

        string Species { get; }

        string FarmId { get; }
    }
}
