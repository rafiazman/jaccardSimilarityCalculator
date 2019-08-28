using System.Collections.Generic;

namespace SimilarityCalculator.Models
{
    public class BagOfWords
    {
        private List<Document> _documents;

        public string NumberOfDocuments { get; set; }
        public string NumberOfWordsInVocabulary { get; set; }
        public string NumberOfNnz { get; set; }

        public List<Document> Documents
        {
            get => _documents ?? (_documents = new List<Document>());
            set => _documents = value;
        }
    }
}