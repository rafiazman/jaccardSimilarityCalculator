using System.Collections.Generic;

namespace SimilarityCalculator.Calculators
{
    public interface ISimilarityCalculator
    {
        (List<(int leftDocumentId, int rightDocumentId, decimal similarity)> similarities, decimal avgSimilarity) CalculateSimilarity();
    }
}