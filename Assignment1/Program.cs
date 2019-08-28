using System;
using System.IO;
using SimilarityCalculator.Calculators;
using SimilarityCalculator.Models;

namespace SimilarityCalculator
{
    public class Program
    {
        private static BagOfWords _bagOfWords;
        private static BagOfWordsReader _fileReader = new BagOfWordsReader();

        public static void Main(string[] args)
        {
            // Open and read bag of words data file
            _bagOfWords = _fileReader.Read();
            
            PrintMenu();
        }

        public static void ExecuteMinHashJaccard()
        {
            bool inputIsInvalid = true;
            int d = 0;

            while (inputIsInvalid)
            {
                Console.Write("Enter value of d (number of hash functions): ");
                if (int.TryParse(Console.ReadLine(), out var userInput) && userInput > 0)
                {
                    d = userInput;
                    inputIsInvalid = false;
                }
                else
                {
                    Console.WriteLine("Invalid d value, please try again.");
                }
            }
            Console.WriteLine();

            // Set output file
            var outputFileStream = new FileStream($"output_minHash_d{d}.csv", FileMode.Create);

            // Instantiate StreamWriter to write to FileStream (filesystem)
            using (var writer = new StreamWriter(outputFileStream))
            {
                Console.WriteLine($"NumDocs: {_bagOfWords.NumberOfDocuments}, NumWords: {_bagOfWords.NumberOfWordsInVocabulary}, NNZ: {_bagOfWords.NumberOfNnz}");
                writer.WriteLine($"NumDocs,{_bagOfWords.NumberOfDocuments},NumWords,{_bagOfWords.NumberOfWordsInVocabulary},NNZ,{_bagOfWords.NumberOfNnz}");

                Console.WriteLine("Calculating Jaccard Similarity (MinHash)...");

                // Initialise MinHash Similarity Calculator
                ISimilarityCalculator minHashCalculator = new SimilarityCalculatorTimer(
                    new MinHashCalculator(_bagOfWords.Documents, d, (decimal) 0.7, 1, 1), writer);

                // Calculate Similarities of all documents
                var results = minHashCalculator.CalculateSimilarity();

                // Write results to file
                foreach (var similarityResult in results.similarities)
                {
                    writer.WriteLine($"D{similarityResult.leftDocumentId},D{similarityResult.rightDocumentId},{similarityResult.similarity}");
                }

                Console.WriteLine($"Average Similarity of all documents: {results.avgSimilarity}");
                writer.WriteLine($"AvgSimilarity (all),{results.avgSimilarity}");

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }

        public static void ExecuteBruteForceJaccard()
        {
            // Set output file
            var outputFileStream = new FileStream("output_bruteForce.csv", FileMode.Create);

            // Instantiate StreamWriter to write to FileStream (filesystem)
            using (var writer = new StreamWriter(outputFileStream))
            {
                Console.WriteLine($"NumDocs: {_bagOfWords.NumberOfDocuments}, NumWords: {_bagOfWords.NumberOfWordsInVocabulary}, NNZ: {_bagOfWords.NumberOfNnz}");
                writer.WriteLine($"NumDocs,{_bagOfWords.NumberOfDocuments},NumWords,{_bagOfWords.NumberOfWordsInVocabulary},NNZ,{_bagOfWords.NumberOfNnz}");

                Console.WriteLine("Calculating Jaccard Similarity (Bruteforce)...");

                // Initialise BruteForce Similarity Calculator
                ISimilarityCalculator bruteForceCalculator = new SimilarityCalculatorTimer(new BruteforceCalculator(_bagOfWords.Documents), writer);

                // Calculate Similarities of all documents
                var results = bruteForceCalculator.CalculateSimilarity();

                // Write results to file
                foreach (var similarityResult in results.similarities)
                {
                    writer.WriteLine($"D{similarityResult.leftDocumentId},D{similarityResult.rightDocumentId},{similarityResult.similarity}");
                }

                Console.WriteLine($"Average Similarity of all documents: {results.avgSimilarity}");
                writer.WriteLine($"AvgSimilarity (all),{results.avgSimilarity}");

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }

        public static void PrintMenu()
        {
            bool goToMenu = true;

            while (goToMenu)
            {
                Console.Clear();
                Console.WriteLine("COMPSCI 753 - Uncertainty in Data\n");
                Console.WriteLine("Jaccard Similarity Calculator by Rafi Azman");
                Console.WriteLine("Program will output results into output_{algorithm}.csv\n");
                Console.WriteLine("To exit program, press Q\n");

                Console.WriteLine("Menu:");
                Console.WriteLine("1. Run Bruteforce Calculation");
                Console.WriteLine("2. Run MinHash Calculation\n");
                var input = Console.ReadKey(true);

                switch (char.ToLowerInvariant(input.KeyChar))
                {
                    case '1':
                        ExecuteBruteForceJaccard();
                        break;
                    case '2':
                        ExecuteMinHashJaccard();
                        break;
                    case 'q':
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
