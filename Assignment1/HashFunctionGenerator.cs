using System;

namespace SimilarityCalculator
{
    public class HashFunctionGenerator
    {
        /// <summary>Generates the specified n.</summary>
        /// <param name="n">The number of hash functions</param>
        /// <param name="universalSetSize">Size of the universal set.</param>
        /// <returns>Array of hash functions</returns>
        public static Func<int, int>[] Generate(int n, int universalSetSize)
        {
            Random rnd = new Random();
            var arr = new Func<int, int>[n];

            for (int i = 0; i < n; i++)
            {
                var a = rnd.Next(0, universalSetSize);
                var b = rnd.Next(0, universalSetSize);

                arr[i] = x => (a * x + b) % universalSetSize;
            }

            return arr;
        }
    }
}