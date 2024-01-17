using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WordFinderContracts
{
    /// <summary>
    /// Base class with most of the functionality required to
    /// implement different WordFinder classes.
    /// </summary>
    public abstract class WordFinderBase : IWordFinder
    {
        #region Private readonly vars

        /// <summary>
        /// This hashtable contains all characters of the matrix
        /// and their coordinates to avoid cycle on it if you
        /// need some coordinates for a specific character
        /// </summary>
        private readonly Hashtable _map = new Hashtable();

        /// <summary>
        /// Matrix with all characters
        /// </summary>
        protected readonly int[,] Matrix;

        /// <summary>
        /// Contains size of the matrix
        /// </summary>
        private readonly int _matrixLength;

        #endregion

        #region Constructor
        /// <summary>
        /// The WordFinder constructor receives a set of strings which represents a character matrix.
        /// The matrix size does not exceed 64x64, all strings contain the same number of characters
        /// </summary>
        /// <param name="matrix">Input matrix</param>
        /// <exception cref="ArgumentNullException">Returned when input matrix is null</exception>
        /// <exception cref="ArgumentException">Returned if some value of input matrix is not appropriated</exception>
        protected WordFinderBase(IEnumerable<string> matrix)
        {
            // To avoid multiple enumerations
            matrix = matrix as IReadOnlyCollection<string> ?? matrix?.ToList();

            #region Validations

            // Get name of param
            const string paramName = nameof(matrix);

            // Matrix can't be null
            if (matrix == null) throw new ArgumentNullException(paramName, Errors.MatrixCannotBeNull);

            // Matrix can't be empty
            if (!matrix.Any()) throw new ArgumentException(paramName, Errors.MatrixCannotBeEmpty);

            // Max number of elements is defined on WordFinderConstants.MatrixElementsMaxSize
            var matrixElementsCannotExceedSize = string.Format(Errors.MatrixElementsCannotExceedSize, WordFinderConstants.MatrixElementsMaxSize);
            if (matrix.Count() > WordFinderConstants.MatrixElementsMaxSize) throw new ArgumentException(paramName, matrixElementsCannotExceedSize);

            // Max size for elements is defined on WordFinderConstants.MatrixElementMaxSize
            var errorMessages = new List<string>();
            var elementsExceededOnSize = matrix.Where(m => m.Length > WordFinderConstants.MatrixElementMaxSize).ToList();
            for (var i = 0; i < elementsExceededOnSize.Count; i++)
            {
                errorMessages.Add(string.Format(Errors.MatrixElementCannotExceedSize, i, WordFinderConstants.MatrixElementsMaxSize));
            }
            if (errorMessages.Any()) throw new ArgumentException(paramName, string.Join(Environment.NewLine, errorMessages));

            // All strings contain the same number of characters
            var hasElementsWithDifferentSize = matrix.Select(m => m.Length).Distinct().Count() > 1;
            if (hasElementsWithDifferentSize) throw new ArgumentException(Errors.MatrixAllElementsMustHaveSameSize, paramName);

            #endregion

            #region Create characters map and matrix
            var y = 0;
            _matrixLength = matrix.Count();
            Matrix = new int[_matrixLength, _matrixLength];
            foreach (var matrixLine in matrix)
            {
                var x = 0;
                foreach (var matrixChar in matrixLine)
                {
                    var matrixCharLowerCase = char.ToLowerInvariant(matrixChar);
                    var currentPositions = _map[matrixCharLowerCase] as List<int[]> ?? new List<int[]>();
                    currentPositions.Add(new[] { x, y });
                    _map[matrixCharLowerCase] = currentPositions;
                    Matrix[x, y] = matrixChar;
                    x++;
                }
                y++;
            }
            #endregion
        }
        #endregion

        #region Protected methods

        /// <summary>
        /// Returns details about the locations of the word on the matrix
        /// </summary>
        /// <param name="word">Word to search by</param>
        /// <returns>Details about the word: coordinates, directions, matches, etc.</returns>
        protected WordFindResult FindWord(string word)
        {
            var result = new WordFindResult
            {
                Word = word,
                Founds = new List<WordFindResultDetail>()
            };

            // Get current character
            var character = word[0];

            // Get coordinates on the matrix for current and next characters
            var currentCharacterCoordinates = GetCoordinates(character);

            // If character is not present on Matrix, nothing to do
            if (!currentCharacterCoordinates.Any()) return result;

            // Check all existing coordinates for current character
            foreach (var fromCoordinates in currentCharacterCoordinates.Where(p => p != null))
            {
                // Search in all directions
                foreach (var direction in WordFinderDirectionExtensions.GetAll())
                {
                    // Try to obtain end coordinates for the word
                    var toCoordinates = GetEndCoordinates(word, fromCoordinates, direction);
                    if (toCoordinates != null)
                    {
                        result.Founds.Add(new WordFindResultDetail
                        {
                            Direction = direction,
                            From = fromCoordinates,
                            To = toCoordinates
                        });
                    }
                }
            }
            return result;
        } 

        #endregion

        #region Private methods

        /// <summary>
        /// Returns coordinates on matrix for the character desired
        /// </summary>
        /// <param name="character">Character to check by the coordinates</param>
        /// <returns></returns>
        private List<int[]> GetCoordinates(char character)
        {
            var characterLowerCase = char.ToLowerInvariant(character);
            return _map[characterLowerCase] as List<int[]> ?? new List<int[]>();
        }

        /// <summary>
        /// Recursive call to get final coordinates for the word, is not present
        /// complete, then returns null
        /// </summary>
        /// <param name="word">Word to search for</param>
        /// <param name="startOn">Initial coordinates</param>
        /// <param name="direction">Direction for the search</param>
        /// <returns></returns>
        private int[] GetEndCoordinates(string word, int[] startOn, WordFinderDirection direction)
        {
            var x = startOn[0];
            var y = startOn[1];

            if (word.Length == 1) return new[] { x, y };

            var isPresent = false;
            var nextCharacter = word[1];
            int[] nextCoordinates = { };

            switch (direction)
            {
                case WordFinderDirection.LeftToRight:
                    var nextPositionX = x + 1;
                    nextCoordinates = new[] { nextPositionX, y };
                    isPresent = nextPositionX <= _matrixLength - 1 && Matrix[nextPositionX, y] == nextCharacter;
                    break;

                case WordFinderDirection.TopToBottom:
                    var nextPositionY = y + 1;
                    nextCoordinates = new[] { x, nextPositionY };
                    isPresent = nextPositionY <= _matrixLength - 1 && Matrix[x, nextPositionY] == nextCharacter;
                    break;
            }

            // If next character is present on same direction continue trying to complete word
            if (isPresent)
            {
                return GetEndCoordinates(word.Substring(1, word.Length - 1), nextCoordinates, direction);
            }

            return null;
        }

        #endregion

        #region Abstract methods
        /// <summary>
        /// The "Find" method should return the top 10 most repeated words from the word stream found in the matrix.
        /// If no words are found, the "Find" method should return an empty set of strings.
        /// If any word in the word stream is found more than once within the stream, the search results should count it only once  
        /// </summary>
        /// <param name="wordStream">Stream of words to be checked</param>
        /// <returns></returns>
        public abstract IEnumerable<WordFindResult> Find(IEnumerable<string> wordStream);
        #endregion
    }
}
