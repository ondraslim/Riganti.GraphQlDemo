using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public class FarmConnection
        : IFarmConnection
    {
        public FarmConnection(
            IReadOnlyList<IFarm> nodes)
        {
            Nodes = nodes;
        }

        public IReadOnlyList<IFarm> Nodes { get; }
    }
}
