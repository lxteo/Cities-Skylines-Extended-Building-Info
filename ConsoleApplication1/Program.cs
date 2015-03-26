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
            var markov = new ExtendedBuildings.Markov(ExtendedBuildings.Properties.Resources.descriptionsWorkplaces, false, 6);
            //var markov = new ExtendedBuildings.Markov("ExtendedBuildings.markov.descriptionsWorkplaces.txt", false, 6);
            Console.Write(markov.GetText(120, 220, true));
            var x = Console.ReadLine();
            if (x == "")
            {
            }
        }
    }
}
