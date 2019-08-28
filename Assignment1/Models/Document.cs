using System.Collections.Generic;

namespace SimilarityCalculator.Models
{
    public class Document
    {
        public int Id { get; set; }
        public List<Word> Words { get; set; }
    }
}