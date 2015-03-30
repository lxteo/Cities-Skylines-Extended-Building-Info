using ColossalFramework.Math;
using ExtendedBuildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var markov = new Markov(ExtendedBuildings.Properties.Resources.nameResidential, false, 4);
            //var markov = new ExtendedBuildings.Markov("ExtendedBuildings.markov.descriptionsWorkplaces.txt", false, 6);
            Randomizer randomizer = new Randomizer(50);
            for (var i = 0; i < 100; i += 1)
            {
                Console.Write(markov.GetText(ref randomizer, 6, 20, true, true));
            }
                Console.Write(markov.GetText(120, 220, true));
            var x = Console.ReadLine();
            if (x == "")
            {
            }
        }
    }
}
