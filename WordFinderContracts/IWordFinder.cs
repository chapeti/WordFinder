using System.Collections.Generic;

namespace WordFinderContracts
{
    public interface IWordFinder
    {
        /// <summary>
        /// "Find" method should return the top X most repeated words from the word stream found in the matrix.
        /// If no words are found, the "Find" method should return an empty set of strings.
        /// If any word in the word stream is found more than once within the stream, the search results should count it only once  
        /// </summary>
        /// <param name="wordStream">A large stream of words</param>
        /// <returns></returns>
        IEnumerable<WordFindResult> Find(IEnumerable<string> wordStream);
    }
}
