using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public class Farm
        : IFarm
    {
        public Farm(
            string id, 
            string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}
