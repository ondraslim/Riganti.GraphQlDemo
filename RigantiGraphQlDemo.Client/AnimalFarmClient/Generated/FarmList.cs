using System;
using System.Collections;
using System.Collections.Generic;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public class FarmList
        : IFarmList
    {
        public FarmList(
            IFarmConnection farms)
        {
            Farms = farms;
        }

        public IFarmConnection Farms { get; }
    }
}
