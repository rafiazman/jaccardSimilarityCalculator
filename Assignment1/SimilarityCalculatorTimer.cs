using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SimilarityCalculator.Calculators;

namespace SimilarityCalculator
{
    public class SimilarityCalculatorTimer : ISimilarityCalculator
    {
        public SimilarityCalculatorTimer(ISimilarityCalculator calculator, StreamWriter streamWriter)
        {
            Calculator = calculator;
            StreamWriter = streamWriter;
        }

        private ISimilarityCalculator Calculator { get; }
        private StreamWriter StreamWriter { get; }

        public (List<(int leftDocumentId, int rightDocumentId, decimal similarity)> similarities, decimal avgSimilarity) CalculateSimilarity()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = Calculator.CalculateSimilarity();

            stopwatch.Stop();

            Console.WriteLine($"Finished execution in: {stopwatch.ElapsedTicks / (decimal) TimeSpan.TicksPerMillisecond}ms");
            StreamWriter.WriteLine($"Finished execution in: {stopwatch.ElapsedTicks / (decimal)TimeSpan.TicksPerMillisecond}ms");

            return result;
        }
    }
}