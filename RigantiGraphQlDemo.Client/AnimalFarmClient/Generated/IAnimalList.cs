using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public interface IAnimalList
    {
        IAnimalConnection Animals { get; }
    }
}
