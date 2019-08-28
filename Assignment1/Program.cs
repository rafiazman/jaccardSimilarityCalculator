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

        public static void ExecuteLsh()
        {
            bool inputIsInvalid = true;
            int k = 0;
            int r = 0;

            while (inputIsInvalid)
            {
                Console.Clear();
                Console.Write("Enter value of k (number of hash functions): ");
                if (int.TryParse(Console.ReadLine(), out var userInput) && userInput > 0)
                {
                    k = userInput;
                }
                else
                {
                    Console.WriteLine("Invalid d value, please try again.");
                    continue;
                }

                Console.Write("Enter value of r (number of rows in each band): ");
                if (int.TryParse(Console.ReadLine(), out var rInput) && userInput > 0)
                {
                    r = rInput;
                }
                else
                {
                    Console.WriteLine("Invalid r value, please try again.");
                    continue;
                }

                Console.WriteLine($"k: {k}, r: {r}, b: {k / r}");
                Console.Write("Confirm selection by pressing Enter...");

                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    inputIsInvalid = false;
                }
            }
            Console.WriteLine();

            // Set output file
            var outputFileStream = new FileStream($"output_lsh_d{k}.csv", FileMode.Create);

            // Instantiate StreamWriter to write to FileStream (filesystem)
            using (var writer = new StreamWriter(outputFileStream))
            {
                Console.WriteLine($"NumDocs: {_bagOfWords.NumberOfDocuments}, NumWords: {_bagOfWords.NumberOfWordsInVocabulary}, NNZ: {_bagOfWords.NumberOfNnz}\n");
                writer.WriteLine($"NumDocs,{_bagOfWords.NumberOfDocuments},NumWords,{_bagOfWords.NumberOfWordsInVocabulary},NNZ,{_bagOfWords.NumberOfNnz}\n");

                Console.WriteLine($"k: {k}, r: {r}, b: {k / r}");

                Console.WriteLine("Calculating Jaccard Similarity (LSH)...");

                // Initialise MinHash Similarity Calculator
                ISimilarityCalculator minHashCalculator = new SimilarityCalculatorTimer(
                    new LocalitySensitiveHashingCalculator(_bagOfWords.Documents, k, r), writer);

                // Calculate Similarities of all documents
                var results = minHashCalculator.CalculateSimilarity();

                // Write results to file
                foreach (var similarityResult in results.similarities)
                {
                    writer.WriteLine($"D{similarityResult.leftDocumentId},D{similarityResult.rightDocumentId},{similarityResult.similarity}");
                }

                Console.WriteLine($"Average Similarity of all documents: {results.avgSimilarity}");
                writer.WriteLine($"AvgSimilarity (all),{results.avgSimilarity}");
                Console.WriteLine($"Output {results.similarities.Count} comparisons.");
                writer.WriteLine($"Output {results.similarities.Count} comparisons.");

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
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
                ISimilarityCalculator minHashCalculator = new SimilarityCalculatorTimer(new MinHashCalculator(_bagOfWords.Documents, d), writer);

                // Calculate Similarities of all documents
                var results = minHashCalculator.CalculateSimilarity();

                // Write results to file
                foreach (var similarityResult in results.similarities)
                {
                    writer.WriteLine($"D{similarityResult.leftDocumentId},D{similarityResult.rightDocumentId},{similarityResult.similarity}");
                }

                Console.WriteLine($"Average Similarity of all documents: {results.avgSimilarity}");
                writer.WriteLine($"AvgSimilarity (all),{results.avgSimilarity}");
                Console.WriteLine($"Output {results.similarities.Count} comparisons.");
                writer.WriteLine($"Output {results.similarities.Count} comparisons.");

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
                Console.WriteLine($"Output {results.similarities.Count} comparisons.");
                writer.WriteLine($"Output {results.similarities.Count} comparisons.");

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
                Console.WriteLine("2. Run MinHash Calculation");
                Console.WriteLine("3. Run LSH Calculation\n");
                var input = Console.ReadKey(true);

                switch (char.ToLowerInvariant(input.KeyChar))
                {
                    case '1':
                        ExecuteBruteForceJaccard();
                        break;
                    case '2':
                        ExecuteMinHashJaccard();
                        break;
                    case '3':
                        ExecuteLsh();
                        break;
                    case 'q':
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
