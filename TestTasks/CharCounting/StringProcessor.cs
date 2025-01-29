using System;
using System.Collections.Generic;

namespace TestTasks.VowelCounting
{
    public class StringProcessor
    {
        public (char symbol, int count)[] GetCharCount(string veryLongString, char[] countedChars)
        {
            Dictionary<char, int> dictionary = new();

            foreach (char c in countedChars)
            {
                dictionary.Add(c, 0);
            }

            foreach (char c in veryLongString)
            {
                if (dictionary.ContainsKey(c))
                {
                    dictionary[c]++;
                }
            }

            (char symbol, int count)[] result = new (char, int)[dictionary.Count];
            int index = 0;

            foreach (var x in dictionary)
            {
                result[index] = (x.Key, x.Value);
                index++;
            }

            return result;
        }
    }
}
