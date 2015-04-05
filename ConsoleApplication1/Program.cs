using ColossalFramework.Globalization;
using ColossalFramework.Math;
using ExtendedBuildings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var markov = new Markov("nameResidential", false, 4);
            //var markov = new ExtendedBuildings.Markov("ExtendedBuildings.markov.descriptionsWorkplaces.txt", false, 6);
            Randomizer randomizer = new Randomizer(50);
            for (var i = 0; i < 100; i += 1)
            {
                Console.WriteLine(markov.GetText(ref randomizer, 6, 13, true, true));
            }
                //Console.Write(markov.GetText(120, 220, true));
            var x = Console.ReadLine();
            if (x == "")
            {
            }
        }
        private static string LoadText(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var locale = LocaleManager.cultureInfo;
            var resourceName = String.Format("ExtendedBuildings.Markov.{0}.{1}.txt", "pt-PT", fileName);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
