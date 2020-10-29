using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public interface IFarm
    {
        string Id { get; }

        string Name { get; }
    }
}
