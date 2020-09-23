using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public interface IFarmConnection
    {
        IReadOnlyList<IFarm> Nodes { get; }
    }
}
