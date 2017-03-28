using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BuzzerBoxDataRetrieval.Helpers
{
    public static class ExtensionMethods
    {
        public static void AddRange<T, U>(this Dictionary<T, U> dictionary, Dictionary<T, U> range)
        {
            if (range == null || dictionary == null)
                return;
            foreach(var item in range)
                dictionary.Add(item.Key, item.Value);
        }
    }
}