using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SimilarityCalculator.Models;

namespace SimilarityCalculator.Calculators
{
    public class MinHashCalculator : ISimilarityCalculator
    {
        /// <summary>Initializes a new instance of the <see cref="MinHashCalculator"/> class.</summary>
        /// <param name="documents">The documents to compare similarity.</param>
        /// <param name="numberOfHashFunctions">The number of MinHash functions.</param>
        public MinHashCalculator(IList<Document> documents, int numberOfHashFunctions)
        {
            Documents = documents;
            NumberOfHashFunctions = numberOfHashFunctions;
        }

        private Func<int, int>[] HashFunctions { get; set; }
        private IList<Document> Documents { get; }
        private int NumberOfHashFunctions { get; }

        public (List<(int leftDocumentId, int rightDocumentId, decimal similarity)> similarities, decimal avgSimilarity) CalculateSimilarity()
        {
            // Get word IDs from all documents
            var universalSet = Documents.SelectMany(d =>
            {
                var words = d.Words.Select(x => x.Id);
                return words;
            }).Distinct().ToArray();

            // Get Boolean Matrix for each document
            var documentBooleanMatrices = Documents.Select(d =>
            {
                var wordIds = d.Words.Select(w => w.Id).ToArray();
                var boolMatrix = GetBooleanMatrix(universalSet, wordIds);

                return boolMatrix;
            }).ToArray();

            // Unify boolean matrices into a single boolean matrix
            var documentBooleanMatrix = new int[universalSet.Length, Documents.Count];

            for (int docIndex = 0; docIndex < documentBooleanMatrix.GetLength(1); docIndex++)
            {
                for (int elIndex = 0; elIndex < documentBooleanMatrix.GetLength(0); elIndex++)
                {
                    documentBooleanMatrix[elIndex, docIndex] = documentBooleanMatrices[docIndex][elIndex];
                }
            }

            // Get MinHash signature matrix
            var minHashSignatureMatrix = GetMinHashSignatureMatrix(documentBooleanMatrix);

            // Compare with all documents in list
            var results = new List<(int leftDocumentId, int rightDocumentId, decimal similarity)>();
            var sumOfSimilarities = (decimal) 0;

            for (int docIndex = 0; docIndex < minHashSignatureMatrix.GetLength(1); docIndex++)
            {
                for (int nextDocIndex = docIndex + 1; nextDocIndex < minHashSignatureMatrix.GetLength(1); nextDocIndex++)
                {
                    var matchingSignatures = 0;
                    var leftDocumentId = Documents[docIndex].Id;
                    var rightDocumentId = Documents[nextDocIndex].Id;

                    for (int minHashIndex = 0; minHashIndex < minHashSignatureMatrix.GetLength(0); minHashIndex++)
                    {
                        if (minHashSignatureMatrix[minHashIndex, docIndex] ==
                            minHashSignatureMatrix[minHashIndex, nextDocIndex])
                        {
                            matchingSignatures++;
                        }
                    }

                    var similarity = (decimal) matchingSignatures / minHashSignatureMatrix.GetLength(0);
                    sumOfSimilarities += similarity;

                    results.Add((leftDocumentId, rightDocumentId, similarity));
                }
            }

            return (results, sumOfSimilarities / results.Count);
        }
        
        public int[] GetBooleanMatrix(int[] universalSet, int[] wordIds)
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
        
        public double[,] GetMinHashSignatureMatrix(int[,] boolMatrix)
        {
            HashFunctions = HashFunctionGenerator.Generate(NumberOfHashFunctions, boolMatrix.GetLength(0));

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