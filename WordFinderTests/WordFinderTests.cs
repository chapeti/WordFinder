using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordFinderContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordFinderImplementation;
using WordFinderTests.Helpers;

namespace WordFinderTests
{
    [TestClass]
    public class WordFinderTests
    {
        [TestMethod]
        [TestCategory("Validations")]
        public void TestConstructorValidations()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { _ = new WordFinder(null); }, "Matrix can't be null");

            Assert.ThrowsException<ArgumentException>(() => { _ = new WordFinder(new List<string>()); }, "Matrix can't be empty");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new WordFinder(TestsHelper.GenerateRandomMatrix(WordFinderConstants.MatrixElementMaxSize + 1, 1));
            }, "Max number of elements is defined on WordFinderConstants.MatrixElementsMaxSize");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new WordFinder(TestsHelper.GenerateRandomMatrix(1, WordFinderConstants.MatrixElementMaxSize + 1));
            }, "Max size for elements is defined on WordFinderConstants.MatrixElementMaxSize");

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new WordFinder(TestsHelper.GenerateRandomMatrixWithElementsOfDifferentSizes(WordFinderConstants.MatrixElementMaxSize, 1, WordFinderConstants.MatrixElementMaxSize));
            }, "All strings contain the same number of characters");
        }

        [TestMethod]
        [TestCategory("Results")]
        public void TestFindMethod()
        {
            TestsHelper.Log("Starting test for WordFinder.Find() method.");

            const int minimumWordSize = 10;
            const int matricesToGenerate = 5;
            const int wordsForMatrix = 100;
            const int repeatedWordsOnMatrix = 10;
            const int repeatedWordsTimes = 5;
            const int matrixSize = 64;

            var words = TestsHelper.GetStrings().Where(w => w.Length >= minimumWordSize);
            var wordsCount = words.Count();

            TestsHelper.Log($"Testing with {matricesToGenerate} matrices of {matrixSize} of size with {wordsForMatrix} words conforming the matrix and {wordsCount} words as input.");

            for (var i = 0; i < matricesToGenerate; i++)
            {
                TestsHelper.Log($"Test {i+1} of {matricesToGenerate}. Generating matrix of {matrixSize}x{matrixSize}...");
                // Get some random words to generate the matrix
                var wordsToGenerateMatrix = words.OrderBy(x => Guid.NewGuid()).Take(wordsForMatrix).ToList();

                // Duplicate words expected into results as they are going to be duplicated on the matrix
                var wordsRepeatedForTheMatrix = wordsToGenerateMatrix.OrderBy(x => Guid.NewGuid()).Take(repeatedWordsOnMatrix).ToList();
                foreach (var wordExpected in wordsRepeatedForTheMatrix.ToList())
                {
                    for (var j = 0; j < repeatedWordsTimes; j++)
                    {
                        wordsRepeatedForTheMatrix.Add(wordExpected);
                    }
                }
                
                // Generate the matrix 
                var matrix = new MatrixGenerator(matrixSize).Generate(wordsToGenerateMatrix, wordsRepeatedForTheMatrix);
                TestsHelper.Log("Matrix generated: ");
                foreach (var line in matrix)
                {
                    TestsHelper.Log(line);
                }

                // Create WordFinder instance and attach to matrix
                var wordFinder = new WordFinder(matrix);

                var timer = new Stopwatch();
                timer.Start();
                TestsHelper.Log($"Starting processing of {wordsCount} words...");
                    
                // Find top most repeated words but using all words on test file,
                // not only words for the matrix creation
                var results = wordFinder.Find(words).ToList();

                timer.Stop();
                TestsHelper.Log($"Process of {wordsCount} words finished in {timer.Elapsed}:");
                foreach (var wordFindResult in results)
                {
                    TestsHelper.Log($"\t - {wordFindResult}");
                }

                // Do we have some of the expected words on the results?
                var wordExpectedToBeOnTheResults =  wordsRepeatedForTheMatrix.Distinct().ToList();
                var expectedResultsArePresent = wordExpectedToBeOnTheResults.Any(r => results.Any(e => e.Word == r));
                TestsHelper.Log("Expected results are present, processing next test.", true);

                Assert.IsTrue(expectedResultsArePresent);
            }
        }
    }
}
