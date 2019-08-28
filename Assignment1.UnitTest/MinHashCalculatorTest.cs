using System.Collections.Generic;
using SimilarityCalculator.Calculators;
using SimilarityCalculator.Models;
using Xunit;

namespace Assignment1.UnitTest
{
    public class MinHashCalculatorTest
    {
        public MinHashCalculatorTest()
        {
            Documents = new List<Document>
            {
                new Document
                {
                    Id = 1,
                    Words = new List<Word>
                    {
                        new Word {Id = 1, Count = 1},
                        new Word {Id = 2, Count = 1},
                        new Word {Id = 5, Count = 1},
                        new Word {Id = 6, Count = 1},
                        new Word {Id = 7, Count = 1}
                    }
                },
                new Document
                {
                    Id = 2,
                    Words = new List<Word>
                    {
                        new Word {Id = 1, Count = 1},
                        new Word {Id = 2, Count = 1},
                        new Word {Id = 3, Count = 1},
                        new Word {Id = 6, Count = 1},
                    }
                },
            };
            MinHashCalculator = new MinHashCalculator(Documents, 100, (decimal) 0.7, 1, 1);
        }

        private IList<Document> Documents { get; }
        private MinHashCalculator MinHashCalculator { get; }


        [Fact]
        public void Test()
        {
            int[,] a = {
                {1, 0},
                {0, 1 },
                {1, 1 },
                {1, 0 },
                {0, 1 }
            };
            var result = MinHashCalculator.CalculateSimilarity();
        }
    }
}
