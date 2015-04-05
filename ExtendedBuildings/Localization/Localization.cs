using ColossalFramework.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExtendedBuildings
{
        public enum LocalizationCategory
        {
            Markov,
            BuildingInfo,
            ServiceInfo,
        }
    class Localization
    {
        private static Dictionary<string, string> texts;

        public static string Get(LocalizationCategory cat, string name)
        {
            if (cat == LocalizationCategory.Markov)
            {
                return GetResource(name);
            }
            else
            {
                if (texts == null)
                {
                     SetText();
                }
                var key = cat.ToString().ToLower() + "_" + name.ToLower();
                if (!texts.ContainsKey(key))
                {
                    return name;
                }
                return texts[key];
            }
        }

        private static void SetText()
        {
            var res = GetResource("text").Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            texts = new Dictionary<string, string>();
            var header = "";
            foreach (var line in res)
            {
                if (line == null || line.Trim().Length == 0)
                {
                    continue;
                }
                
                var firstSpace = line.Trim().IndexOf(' ');
                if (firstSpace == -1)
                {
                    header = line.Trim().Replace(":", "").ToLower();
                }
                else
                {
                    texts.Add(header + "_" + line.Substring(0, firstSpace).ToLower(), line.Substring(firstSpace + 1).Trim());
                }                
            }
        }

        private static string GetResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var locale = LocaleManager.instance.language;
            if (locale == null)
            {
                locale = "en";
            }

            var rn = String.Format("ExtendedBuildings.Localization.{0}.{1}.txt", locale.Trim().ToLower(), resourceName);
            if (!assembly.GetManifestResourceNames().Contains(rn))
            {
                rn = String.Format("ExtendedBuildings.Localization.{0}.{1}.txt", "en", resourceName);
            }            
            using (Stream stream = assembly.GetManifestResourceStream(rn))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
