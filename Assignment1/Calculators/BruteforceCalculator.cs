using System.Collections.Generic;
using System.Linq;
using SimilarityCalculator.Models;

namespace SimilarityCalculator.Calculators
{
    public class BruteforceCalculator : ISimilarityCalculator
    {
        public BruteforceCalculator(IList<Document> documents)
        {
            Documents = documents;
        }

        private IList<Document> Documents { get; }

        public (List<(int leftDocumentId, int rightDocumentId, decimal similarity)> similarities, decimal avgSimilarity) CalculateSimilarity()
        {
            var results = new List<(int leftDocumentId, int rightDocumentId, decimal similarity)>();
            var sumOfSimilarities = (decimal) 0;

            // Compare with all documents in list
            for (int i = 0; i < Documents.Count - 1; i++)
            {
                for (int j = i + 1; j < Documents.Count; j++)
                {
                    var leftDocument = Documents[i];
                    var leftDocumentId = Documents[i].Id;

                    var rightDocument = Documents[j];
                    var rightDocumentId = Documents[j].Id;

                    var similarity = CalculateSimilarity(leftDocument, rightDocument);

                    results.Add((leftDocumentId, rightDocumentId, similarity));
                    sumOfSimilarities += similarity;
                }
            }

            return (results, sumOfSimilarities / results.Count);
        }
        private decimal CalculateSimilarity(Document a, Document b)
        {
            var leftWordIds = a.Words.Select(x => x.Id).ToList();
            var rightWordIds = b.Words.Select(x => x.Id).ToList();
            var universalSet = leftWordIds.Union(rightWordIds);

            var matches = 0;

            foreach (var leftWord in leftWordIds)
            {
                foreach (var rightWord in rightWordIds)
                {
                    if (leftWord == rightWord) matches += 1;
                }
            }

            return (decimal) matches / universalSet.Count();
        }
    }
}