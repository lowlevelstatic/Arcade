using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class ListExtensions
    {
        private static readonly Random _random = new();  

        public static T PickRandom<T>(this List<T> source) => source.PickRandom(_random);
        
        public static T PickRandom<T>(this List<T> source, Random random) => source[random.Next(source.Count)];

        public static List<T> Shuffle<T>(this List<T> source) => source.Shuffle(_random);
        
        public static List<T> Shuffle<T>(this List<T> source, Random random)
        {
            int count = source.Count;  

            while (count > 1)
            {
                count--;
                int k = random.Next(count + 1);
                (source[k], source[count]) = (source[count], source[k]);
            }

            return source;
        }
    }
}