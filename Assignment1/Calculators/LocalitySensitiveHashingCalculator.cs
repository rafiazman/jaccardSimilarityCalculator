using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SimilarityCalculator.Models;

namespace SimilarityCalculator.Calculators
{
    public class LocalitySensitiveHashingCalculator : ISimilarityCalculator
    {
        private readonly int _r;

        /// <summary>Initializes a new instance of the <see cref="LocalitySensitiveHashingCalculator"/> class.</summary>
        /// <param name="documents">The documents to compare similarity.</param>
        /// <param name="k">The number of MinHash functions.</param>
        /// <param name="r">The r value/the row size of b band</param>
        public LocalitySensitiveHashingCalculator(IList<Document> documents, int k, int r)
        {
            Documents = documents;
            UniversalSet = documents.SelectMany(d =>
            {
                var words = d.Words.Select(x => x.Id);
                return words;
            }).Distinct().ToArray();

            HashFunctions = HashFunctionGenerator.Generate(k, UniversalSet.Length);

            _r = r;
        }

        private Func<int, int>[] HashFunctions { get; }
        private int[] UniversalSet { get; }
        private IList<Document> Documents { get; }

        public (List<(int leftDocumentId, int rightDocumentId, decimal similarity)> similarities, decimal avgSimilarity) CalculateSimilarity()
        {
            // Get Boolean Matrix for each document
            var documentBooleanMatrices = Documents.Select(d =>
            {
                var wordIds = d.Words.Select(w => w.Id).ToArray();
                var boolMatrix = GetBooleanMatrix(UniversalSet, wordIds);

                return boolMatrix;
            }).ToArray();

            // Unify boolean matrices into a single boolean matrix
            var documentBooleanMatrix = new int[UniversalSet.Length, Documents.Count];

            for (int docIndex = 0; docIndex < documentBooleanMatrix.GetLength(1); docIndex++)
            {
                for (int elIndex = 0; elIndex < documentBooleanMatrix.GetLength(0); elIndex++)
                {
                    documentBooleanMatrix[elIndex, docIndex] = documentBooleanMatrices[docIndex][elIndex];
                }
            }

            // Get MinHash signature matrix
            var minHashSignatureMatrix = GetMinHashSignatureMatrix(documentBooleanMatrix);


            // Split signature matrix into b bands (number of bands) with r rows (number of rows in one band)
            var bBands = new List<(int docIndex, List<string> bands)>();

            for (int docIndex = 0; docIndex < minHashSignatureMatrix.GetLength(1); docIndex++)
            {
                var bands = new List<string>();
                var row = new StringBuilder();

                for (int minHashIndex = 0; minHashIndex < minHashSignatureMatrix.GetLength(0); minHashIndex++)
                {
                    row.Append(minHashSignatureMatrix[minHashIndex, docIndex]);

                    // If divisible by r or is at end of array
                    if ((minHashIndex + 1) % _r == 0 || minHashIndex == minHashSignatureMatrix.GetLength(0) - 1)
                    {
                        bands.Add(row.ToString());
                        row = new StringBuilder();
                    }
                }

                bBands.Add((docIndex, bands));
            }

            var sumOfSimilarities = (decimal)0;
            var results = new List<(int leftDocumentId, int rightDocumentId, decimal similarity)>();

            // Iterate through b-bands and check for equal bands implying similarity
            for (int i = 0; i < bBands.Count; i++)
            {
                for (int j = i + 1; j < bBands.Count; j++)
                {
                    var documentBands = bBands[i].bands;
                    var nextDocumentBands = bBands[j].bands;

                    foreach (var documentBand in documentBands)
                    {
                        if (nextDocumentBands.Contains(documentBand))
                        {
                            // do minHash comparison
                            var matchingSignatures = 0;
                            var leftDocumentId = Documents[i].Id;
                            var rightDocumentId = Documents[j].Id;

                            for (int minHashIndex = 0; minHashIndex < minHashSignatureMatrix.GetLength(0); minHashIndex++)
                            {
                                if (minHashSignatureMatrix[minHashIndex, i] ==
                                    minHashSignatureMatrix[minHashIndex, j])
                                {
                                    matchingSignatures++;
                                }
                            }

                            var similarity = (decimal)matchingSignatures / minHashSignatureMatrix.GetLength(0);
                            sumOfSimilarities += similarity;

                            results.Add((leftDocumentId, rightDocumentId, similarity));
                        }
                    }
                }
            }

            return (results, sumOfSimilarities / results.Count);
        }

        private int[] GetBooleanMatrix(int[] universalSet, int[] wordIds)
        {
            var booleanMatrix = new int[universalSet.Length];

            for (int i = 0; i < booleanMatrix.Length; i++)
            {
                if (wordIds.Contains(universalSet[i]))
                {
                    booleanMatrix[i] = 1;
                }
                else
                {
                    booleanMatrix[i] = 0;
                }
            }

            return booleanMatrix;
        }

        private double[,] GetMinHashSignatureMatrix(int[,] boolMatrix)
        {
            var minHashStopwatch = Stopwatch.StartNew();
            double[,] minHashes = new double[HashFunctions.Length, boolMatrix.GetLength(1)];

            // Initialise all minhashes as infinity
            for (int i = 0; i < minHashes.GetLength(0); i++)
            {
                for (int j = 0; j < minHashes.GetLength(1); j++)
                {
                    minHashes[i, j] = double.PositiveInfinity;
                }
            }

            // Iterate through boolean matrix
            for (int docIndex = 0; docIndex < boolMatrix.GetLength(1); docIndex++)
            {
                for (int elIndex = 0; elIndex < boolMatrix.GetLength(0); elIndex++)
                {
                    var rowNumber = elIndex + 1;

                    if (boolMatrix[elIndex, docIndex] == 1)
                    {
                        for (int hashFuncIndex = 0; hashFuncIndex < HashFunctions.Length; hashFuncIndex++)
                        {
                            var hashValue = HashFunctions[hashFuncIndex](rowNumber);

                            if (hashValue < minHashes[hashFuncIndex, docIndex])
                            {
                                minHashes[hashFuncIndex, docIndex] = hashValue;
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"MinHash Signatures computed in: {minHashStopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond}ms");
            minHashStopwatch.Stop();

            return minHashes;
        }
    }
}