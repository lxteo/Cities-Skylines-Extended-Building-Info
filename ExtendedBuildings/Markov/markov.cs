using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ColossalFramework.Math;
<<<<<<< HEAD
using ColossalFramework.Globalization;
=======
>>>>>>> lxteo/master
using UnityEngine;

namespace ExtendedBuildings
{
    public class Markov
    {
        private Dictionary<string, Ngram> pairs;
        private Dictionary<string, int> starters;
        int n;
        bool isWord;

        public Markov(string resourceName, bool useWords, int n)
        {
<<<<<<< HEAD

            var assembly = Assembly.GetExecutingAssembly();
            var locale = LocaleManager.cultureInfo;
            if (locale == null)
            {
                locale = new System.Globalization.CultureInfo("en-US");
            }


            resourceName = String.Format("ExtendedBuildings.Markov.{0}.{1}.txt", locale.Name.Substring(0,locale.Name.IndexOf('-')), resourceName);

            if (!assembly.GetManifestResourceNames().Contains(resourceName))
            {
                resourceName = String.Format("ExtendedBuildings.Markov.{0}.{1}.txt", "en", resourceName);
            }

            var resource = "";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                     resource = reader.ReadToEnd();
                }
            }

=======
            var resource = Localization.Get(LocalizationCategory.Markov, resourceName);           
>>>>>>> lxteo/master
            this.n = n;

            pairs = new Dictionary<string, Ngram>();
            starters = new Dictionary<string, int>();

            var buffer = resource.Split(new string[] { "\r\n","\n" }, StringSplitOptions.None);
            foreach (var line in buffer)
            {
                if (line == null || line.Trim().Length <= n)
                {
                    continue;
                }
                if (useWords)
                {
                    GenerateByWord(line.Trim(), n);
                }
                else
                {
                    GenerateByChar(line.Trim(), n);
                }
            }
        }


        private void GenerateByWord(string p, int n)
        {
            isWord = true;
            var line = p.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (line.Length < n)
            {
                return;
            }

            var result = new string[n];
            for (var i = 0; i < line.Length - n; i += 1)
            {
                for (var j = 0; j < n; j += 1)
                {
                    result[j] = line[i + j];
                }
                var rr = String.Join(" ", result);
                if (i == 0)
                {
                    if (!starters.ContainsKey(rr))
                    {
                        starters.Add(rr, 0);
                    }
                    starters[rr] += 1;
                }

                if (!pairs.ContainsKey(rr))
                {
                    pairs.Add(rr, new Ngram());
                }
                if (i + n < line.Length)
                {
                    string next = line[i + n];
                    pairs[rr].Add(next);
                }
                else
                {
                    pairs[rr].Add("");
                }
            }
        }

        private void GenerateByChar(string p, int n)
        {
            isWord = false;
            var line = p.ToCharArray();
            if (line.Length < n)
            {
                return;
            }


            for (var i = 0; i < line.Length - n; i += 1)
            {
                var result = "";
                for (var j = 0; j < n; j += 1)
                {
                    result += line[i + j];
                }

                if (i == 0)
                {
                    if (!starters.ContainsKey(result))
                    {
                        starters.Add(result, 0);
                    }
                    starters[result] += 1;
                }

                if (!pairs.ContainsKey(result))
                {
                    pairs.Add(result, new Ngram());
                }

                if (i + n < line.Length)
                {
                    string next = new String(line, i + n, 1);
                    pairs[result].Add(next);
                }
                else
                {
                    pairs[result].Add("");
                }
            }
        }

        public string GetText(int min, int max, bool useStarter)
        {
            var rand = new Randomizer(DateTime.Now.Ticks);
            var test = rand.Int32(0, 3);
            return GetText(ref rand, min, max, useStarter);
        }

        public string GetText(ref Randomizer rand, int min, int max, bool useStarter,bool endOnSpaces = false)
        {
            var what = rand.Int32(41u);
            var result = new List<string>();
            
            while (result.Count() < min)
            {
                var starter = "";
                if (useStarter)
                {
                    starter = GetRandomStarter(ref rand);
                }
                else
                {
                    starter = pairs.Keys.ElementAt(rand.Int32((uint)pairs.Count()));
                }
                var current = pairs[starter];

                string[] starterSplit;
                if (isWord)
                {
                    starterSplit = starter.Split(' ');
                    foreach (var ss in starterSplit)
                    {
                        result.Add(ss);
                    }
                }
                else
                {
                    foreach (var cc in starter.ToCharArray())
                    {
                        result.Add(cc.ToString());
                    }
                }

                for (var i = 0; i < 1000; i += 1)
                {
                    var next = current.GetNext(ref rand);
                    if (next == "")
                    {
                        break;
                    }
                    result.Add(next);

                    var count = result.Count() - n;
                    var nextCurrent = new string[n];
                    for (var j = 0; j < n; j += 1)
                    {
                        nextCurrent[j] = result[count + j];
                    }

                    var nc = "";
                    if (isWord)
                    {
                        nc = String.Join(" ", nextCurrent);
                    }
                    else
                    {
                        nc = String.Join("", nextCurrent);
                    }

                    if (!pairs.ContainsKey(nc))
                    {
                        break;
                    }
                    current = pairs[nc];
                    var lastWord = result[result.Count - 1];
                    var lastChar = lastWord.Substring(lastWord.Length - 1);
                    if (result.Count > ((max - min) / 2 + min))
                    {
                        var randValue = rand.Int32((uint)(Math.Max(0,max * 2 - result.Count)));
                        if (endOnSpaces && (lastChar == " " && randValue < 3))
                        {
                            break;
                        }
                        if ((lastChar == "."|| lastChar == "!") && randValue < 8){
                            break;
                        }
                    }
                    
                }

                if (!isWord)
                {
                    result.Add(" ");
                }
            }

            if (isWord)
            {
                return string.Join(" ", result.ToArray()).Trim();
            }
            else
            {
                return string.Join("", result.ToArray()).Trim();
            }

        }

        private string GetRandomStarter(ref Randomizer rand)
        {
            var total = starters.Sum(c => c.Value);
            int target = rand.Int32((uint)total);
            target = rand.Int32((uint)total);
            int sum = 0;
            foreach (var kvp in starters)
            {
                sum += kvp.Value;
                if (sum >= target)
                {
                    return kvp.Key;
                }
            }
            return null;
        }

    }

    class Ngram
    {
        public List<string> Following = new List<string>();

        public Ngram()
        {

        }

        internal void Add(string p)
        {
            Following.Add(p);
        }

        public string GetNext(ref Randomizer rand)
        {
            return Following[rand.Int32((uint)Following.Count)];
        }
    }
}
