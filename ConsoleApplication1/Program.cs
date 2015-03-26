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
            var markov = new ExtendedBuildings.Markov("ExtendedBuildings.markov.descriptionsWorkplaces.txt");
            Console.Write(markov.GetText(1400));
            var x = Console.ReadLine();
            if (x=="")
            {
                Console.Write(markov.GetText(1400));
            }
        }
    }
}
