using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SimilarityCalculator.Models;

namespace SimilarityCalculator
{
    public class BagOfWordsReader
    {
        public BagOfWords Read()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .Single(x => x.EndsWith("docword.kos.txt"));

            Console.WriteLine("Opening bag of words...");

            var bagOfWords = new BagOfWords();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                bagOfWords.NumberOfDocuments = sr.ReadLine();
                bagOfWords.NumberOfWordsInVocabulary = sr.ReadLine();
                bagOfWords.NumberOfNnz = sr.ReadLine();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var data = line.Split(" ");

                    var documentId = Convert.ToInt32(data[0]);
                    var wordId = Convert.ToInt32(data[1]);
                    var wordCount = Convert.ToInt32(data[2]);

                    if (bagOfWords.Documents.Exists(x => x.Id == documentId))
                    {
                        var word = new Word
                        {
                            Id = wordId,
                            Count = wordCount
                        };
                        bagOfWords.Documents.Find(x => x.Id == documentId).Words.Add(word);
                        continue;
                    }

                    var document = new Document
                    {
                        Id = documentId,
                        Words = new List<Word>
                        {
                            new Word
                            {
                                Id = wordId,
                                Count = wordCount
                            }
                        }
                    };
                    bagOfWords.Documents.Add(document);
                }
            }
            Console.WriteLine("Bag of words loaded.");

            return bagOfWords;
        }
    }
}