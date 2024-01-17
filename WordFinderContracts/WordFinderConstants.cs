namespace WordFinderContracts
{
    public static class WordFinderConstants
    {
        /// <summary>
        /// The matrix elements size do not exceed this size in characters.
        /// </summary>
        public const int MatrixElementMaxSize = 64;

        /// <summary>
        ///  The matrix size does not exceed this number of elements.
        /// </summary>
        public const int MatrixElementsMaxSize = 64;

        /// <summary>
        /// The "Find" method should return the top X most repeated words from the word stream found in the matrix.
        /// </summary>
        public const int TopMostRepeatedWordsToReturn = 10;
    }
}
