using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedBuildings
{
    public class ExtendedBuildingsMod : IUserMod
    {
            public string Name
            {
                get
                {
                    return "Extended Building Information Mod";
                }
            }
            public string Description
            {
                get
                {
                    return "Displays level up requirements, a random bulding name, and company description.";
                }
            }
    }
}
