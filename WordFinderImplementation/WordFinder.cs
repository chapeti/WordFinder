using System.Collections.Generic;
using System.Linq;
using WordFinderContracts;

namespace WordFinderImplementation
{
    public class WordFinder : WordFinderBase
    {

        #region Constructor
        public WordFinder(IEnumerable<string> matrix) : base(matrix)
        {
        }
        #endregion

        #region IWordFinder
        public override IEnumerable<WordFindResult> Find(IEnumerable<string> wordStream)
        {
            var results = new List<WordFindResult>();

            wordStream.AsParallel().ForAll(word =>
            {
                // Is the word present on the matrix?
                var wordFindResult = FindWord(word);

                // If not, just continue
                if (!wordFindResult.Founds.Any()) return;

                // Increase count for the word
                results.Add(wordFindResult);
            });

            // The "Find" method should return the top 10 most repeated
            // words from the word stream found in the matrix.
            return results.OrderByDescending(r => r.Founds.Count)
                          .Take(WordFinderConstants.TopMostRepeatedWordsToReturn);
        }
        #endregion

    }
}
