using System.Collections.Generic;

namespace Utils 
{
    static public class Random
    {
        private static System.Random rng = new System.Random();
        public static T[] Shuffle<T>(T[] list)
        {
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
