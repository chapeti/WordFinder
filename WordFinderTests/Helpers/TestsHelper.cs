using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WordPuzzle.Classes;

namespace WordFinderTests.Helpers
{
    internal static class TestsHelper
    {
        #region Constants
        internal const string AllowedCharsForRandomStrings = "abcdefghijkmnopqrstuvwxyz";
        #endregion

        #region Static vars
        private static readonly Random RandomForChars = new Random();
        private static readonly Random RandomForElementSize = new Random(); 
        private static readonly DateTime DateTime = DateTime.UtcNow;
        #endregion

        #region Public methods

        /// <summary>
        /// Generates a random matrix for tests
        /// </summary>
        /// <param name="totalElements">Total elements for the matrix</param>
        /// <param name="elementSize">Size of the elements on the matrix</param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateRandomMatrix(int totalElements, int elementSize)
        {
            for (var i = 0; i < totalElements; i++)
            {
                yield return CreateRandomString(elementSize);
            }
        }

        /// <summary>
        /// Generates a random matrix of random size elements for tests
        /// </summary>
        /// <param name="totalElements">Total elements for the matrix</param>
        /// <param name="minElementSize">Min size of the elements on the matrix</param>
        /// <param name="maxElementSize">Max size of the elements on the matrix</param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateRandomMatrixWithElementsOfDifferentSizes(int totalElements, int minElementSize, int maxElementSize)
        {
            for (var i = 0; i < totalElements; i++)
            {
                var elementSize = RandomForElementSize.Next(minElementSize, maxElementSize);
                yield return CreateRandomString(elementSize);
            }
        }

        /// <summary>
        /// Returns a list of strings contained on a file and separated a new line character(s)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetStrings(string fileName = "words.txt")
        {
            using (var reader = File.OpenText(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static void Log(string message, bool extraLine = false, string file = null)
        {
            //message = $"[{DateTime.UtcNow:O}] {message}";

            if (file == null)
            {
                file = $"WordFinderOutput-{DateTime.Year}-{DateTime.Month}-{DateTime.Day}-{DateTime.Hour}{DateTime.Minute}{DateTime.Second}.txt";
            }

            // Output to console
            Debug.WriteLine(message);

            // Output to file
            File.AppendAllText(file, message+Environment.NewLine);

            if (extraLine)
            {
                Debug.WriteLine(Environment.NewLine);
                File.AppendAllText(file, Environment.NewLine);
            }
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Generates a random string 
        /// </summary>
        /// <param name="stringLength">Length for the string generated</param>
        /// <returns></returns>
        private static string CreateRandomString(int stringLength)
        {
            var chars = new char[stringLength];
            for (var i = 0; i < stringLength; i++)
            {
                chars[i] = AllowedCharsForRandomStrings[RandomForChars.Next(0, AllowedCharsForRandomStrings.Length)];
            }
            return new string(chars);
        }

        #endregion
    }

    #region Matrices generator helper
    
    public enum Direction { Down = 1, Right, Left, Up, None };

    /// <summary>
    /// Credits: https://www.codeproject.com/Articles/1271730/Mr-Crossworder-Create-Crosswords-in-Seconds
    ///
    /// This is not part of the test, added just to have a quick way to generate matrices and properly test everything.
    /// </summary>
    public class MatrixGenerator
    {
        public List<RegularWordDetails> WordDetails = new List<RegularWordDetails>();
        private Random _rnd;
        public char[,] Matrix;                          // This is a word matrix to mimic the board.        
        private readonly List<Direction> _directions = new List<Direction>();
        private readonly int _matrixSize;

        public MatrixGenerator(int matrixSize)
        {
            _directions.Add(Direction.Down);
            _directions.Add(Direction.Right);
            _directions.Add(Direction.None);

            _matrixSize = matrixSize;
        }

        /// <summary>
        /// Loops through all the words on the list and tries to find a placement for them.
        /// </summary>
        /// <param name="words">Words to include into the matrix</param>
        /// <param name="wordsToDuplicate">Words to insert duplicated on the matrix and verify results</param>
        public List<string> Generate(IEnumerable<string> words, IEnumerable<string> wordsToDuplicate)
        {
            WordDetails.Clear();
            Matrix = new char[_matrixSize, _matrixSize];
            _rnd = new Random(DateTime.Now.Millisecond);
            var i = -1;

            var allWords = words.Concat(wordsToDuplicate);

            foreach (var word in allWords)
            {
                long attempts = 0;
                bool success;
                i++;
                do
                {
                    var direction = GetDirection(_rnd, 3);
                    var x = GetRandomAxis(_rnd, _matrixSize);
                    var y = GetRandomAxis(_rnd, _matrixSize);
                    success = PlaceTheWord(direction, x, y, word, "", i, ref attempts);
                    if (attempts <= 20000) continue;
                    SaveWordDetailsInCollection(word, "", -1, -1, Direction.None, attempts, true);
                    break;
                }
                while (!success);
            }

            // Convert into our representation for the tests
            var list = new List<string>();
            for (var j = 0; j < Matrix.GetLength(0); j++)
            {
                var line = string.Empty;
                for (var k = 0; k < Matrix.GetLength(1); k++)
                {
                    var randomChar = TestsHelper.AllowedCharsForRandomStrings[_rnd.Next(0, TestsHelper.AllowedCharsForRandomStrings.Length)];
                    var character = Matrix[j, k];
                    var c = character == '\0' ? randomChar.ToString() : character.ToString();
                    line += c;
                }
                list.Add(line);
            }
            return list;

        }
        
        /// <summary>
        /// Randomly generate direction - ACROSS (RIGHT), or DOWN.
        /// </summary>
        /// <param name="Rnd"></param>
        /// <param name="Max"></param>
        /// <returns></returns>
        private Direction GetDirection(Random Rnd, int Max)
        {
            switch (Rnd.Next(1, Max))   // Generate a random number between 1 and Max - 1; So if Max = 9, it will generate a random direction between 1 and 8.
            {
                case 1: if (_directions.Find(p => p.Equals(Direction.Down)) == Direction.Down) return Direction.Down; break;
                case 2: if (_directions.Find(p => p.Equals(Direction.Right)) == Direction.Right) return Direction.Right; break;
                default: return Direction.None;
            }
            return Direction.None;
        }

        /// <summary>
        /// Generates random X or Y axis.
        /// </summary>
        /// <param name="Rnd"></param>
        /// <param name="Max"></param>
        /// <returns></returns>
        private static int GetRandomAxis(Random Rnd, int Max)
        {
            return Rnd.Next(Max);   // Generates a number from 0 up to the grid size.
        }

        /// <summary>
        /// This function checks if a word that is already to the left of the word rightfully passes through the word to be placed.
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="direction">The direction to search for from the (x, y)</param>
        /// <returns></returns>
        private bool LegitimateOverlapOfAnExistingWord(int x, int y, string word, Direction direction)
        {
            char[] chars = new char[_matrixSize];
            int originalX = x, originalY = y;
            try
            {
                switch (direction)
                {
                    case Direction.Left:
                        while (--x >= 0)
                            if (Matrix[x, y] == '\0') break;                                // First walk towards the left until you reach the beginning of the word that is already on the board.
                        ++x;

                        for (int i = 0; x < _matrixSize && i < _matrixSize; x++, i++) // Now walk towards right until you reach the end of the word that is already on the board.
                        {
                            if (Matrix[x, y] == '\0') break;
                            chars[i] = Matrix[x, y];
                        }

                        string str = new string(chars);
                        str = str.Trim('\0');
                        RegularWordDetails wordOnBoard = (RegularWordDetails)WordDetails.Find(a => a.Word == str);  // See if the characters form a valid word that is already on the board.
                        if (wordOnBoard == null) return false;                              // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                        if (wordOnBoard.WordDirection == Direction.Down) return false;      // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                        if (wordOnBoard.X + wordOnBoard.Word.Length == originalX) return false; // The word on the board ended just before the x-cordinate for the current word to place. Hence illegitimate.
                        return true;                                                        // Else, passed all validation checks for a legitimate overlap, hence return true.
                    case Direction.Right:
                        while (--x >= 0)
                            if (Matrix[x, y] == '\0') break;                                // First walk towards the left until you reach the beginning of the word that is already on the board.
                        ++x;

                        for (int i = 0; x < _matrixSize && i < _matrixSize; x++, i++) // Now walk towards right until you reach the end of the word that is already on the board.
                        {
                            if (Matrix[x, y] == '\0') break;
                            chars[i] = Matrix[x, y];
                        }

                        str = new string(chars);
                        str = str.Trim('\0');
                        wordOnBoard = (RegularWordDetails)WordDetails.Find(a => a.Word == str);     // See if the characters form a valid word that is already on the board.
                        if (wordOnBoard == null) return false;                                      // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                        if (wordOnBoard.WordDirection == Direction.Down) return false;              // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                        if (wordOnBoard.X == originalX + 1) return false;                           // The word on the board starts right after the x-cordinate for the current word to place. Hence illegitimate.
                        return true;                                                                // Else, passed all validation checks for a legitimate overlap, hence return true.
                    case Direction.Up:
                        while (--y >= 0)
                            if (Matrix[x, y] == '\0') break;                                        // First walk upwards until you reach the beginning of the word that is already on the board.
                        ++y;

                        for (int i = 0; y < _matrixSize && i < _matrixSize; y++, i++) // Now walk downwards until you reach the end of the word that is already on the board.
                        {
                            if (Matrix[x, y] == '\0') break;
                            chars[i] = Matrix[x, y];
                        }

                        str = new string(chars);
                        str = str.Trim('\0');
                        wordOnBoard = (RegularWordDetails)WordDetails.Find(a => a.Word == str);     // See if the characters form a valid word that is already on the board.
                        if (wordOnBoard == null) return false;                                      // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                        if (wordOnBoard.WordDirection == Direction.Right) return false;             // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                        if (wordOnBoard.Y + wordOnBoard.Word.Length == originalY) return false;     // The word on the board starts right below the y-cordinate for the current word to place. Hence illegitimate.
                        return true;                                                                // Else, passed all validation checks for a legitimate overlap, hence return true.
                    case Direction.Down:
                        while (--y >= 0)
                            if (Matrix[x, y] == '\0') break;                                        // First walk upwards until you reach the beginning of the word that is already on the board.
                        ++y;

                        for (int i = 0; y < _matrixSize && i < _matrixSize; y++, i++) // Now walk downwards until you reach the end of the word that is already on the board.
                        {
                            if (Matrix[x, y] == '\0') break;
                            chars[i] = Matrix[x, y];
                        }

                        str = new string(chars);
                        str = str.Trim('\0');
                        wordOnBoard = (RegularWordDetails)WordDetails.Find(a => a.Word == str);     // See if the characters form a valid word that is already on the board.
                        if (wordOnBoard == null) return false;                                      // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                        if (wordOnBoard.WordDirection == Direction.Right) return false;             // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                        if (wordOnBoard.Y == originalY + 1) return false;                           // The word on the board starts right after the x-cordinate for the current word to place. Hence illegitimate.
                        return true;                                                                // Else, passed all validation checks for a legitimate overlap, hence return true.
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the LegitimateOverlapOfAnExistingWord() method of the 'GameEngine' class.\n\n" +
                                $"Original x = {originalX}, Original y = {originalY}, x = {x}, y = {y}, word: {word}, Direction: {direction}." +
                                $"\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if any left-side characters of the word to be placed is a valid overlap from an existing ACROSS word on the board.
        /// E.g.: CART is to be placed downwards. It checks valid overlaps with existing right-directed words - ARC, PARTY.
        ///     A R C
        ///         A
        ///     P A R T Y
        ///         T
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        /// <returns></returns>
        private bool LeftCellFreeForDownDirectedWord(int x, int y, string word)
        {
            try
            {
                if (x == 0) return true;
                bool isValid = true;
                if (x > 0)
                {
                    for (int i = 0; i < word.Length; y++, i++)
                    {
                        if (Matrix[x - 1, y] != '\0')
                            isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Left);
                        if (!isValid) break;
                    }
                }
                return isValid;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the LeftCellFreeForDownDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if any there is any character to the left cell of the current word to place.
        /// E.g.: CAT is to be placed ACROSS, then there cannot be a letter to the left of CAT.
        /// ****************   P |__|
        /// **************** |__|  C   A   T
        /// ****************   A |__|
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        /// <returns></returns>
        private bool LeftCellFreeForRightDirectedWord(int x, int y, string word)
        {
            try
            {
                if (x == 0) return true;
                if (x - 1 >= 0)
                    return Matrix[x - 1, y] == '\0';
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the LeftCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if any right-side characters of the word to be placed is a valid overlap from an existing ACROSS word on the board.
        /// E.g.: CART is to be placed downwards. It checks valid overlaps with existing right-directed words - ARC, PARTY.
        ///     A R C
        ///         A
        ///     P A R T Y
        ///         T
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        /// <returns></returns>
        private bool RightCellFreeForDownDirectedWord(int x, int y, string word)
        {
            try
            {
                if (x == _matrixSize) return true;
                bool isValid = true;
                if (x + 1 < _matrixSize)
                {
                    for (int i = 0; i < word.Length; y++, i++)
                    {
                        if (Matrix[x + 1, y] != '\0')
                            isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Right);
                        if (!isValid) break;
                    }
                }
                return isValid;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the RightCellFreeForDownwardWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if any there is any character to the left cell of the current word to place.
        /// E.g.: CAT is to be placed ACROSS, then there cannot be a letter to the right of CAT.
        /// ****************       |__|  P
        /// **************** C   A   T  |__|
        /// ****************       |__|
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        /// <returns></returns>
        private bool RightMostCellFreeForRightDirectedWord(int x, int y, string word)
        {
            try
            {
                if (x + word.Length == _matrixSize) return true;
                if (x + word.Length < _matrixSize)
                    return Matrix[x + word.Length, y] == '\0';
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the RightMostCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// If it is a DWON word, then there should not be any character in the adjacent cell to the top of the beginning of this word.
        /// E.g.: If CAT is to be placed downwards, then the top cell should be free:
        /// |___| C L A S S
        ///   C
        ///   A
        ///   T
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        /// <returns></returns>
        private bool TopCellFreeForDownDirectedWord(int x, int y, string word)
        {
            try
            {
                if (y == 0) return true;
                if (y - 1 >= 0)
                    return Matrix[x, y - 1] == '\0';
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the TopCellFreeForDownDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if any top-side characters of the word to be placed is a valid overlap from an existing DOWB word on the board.
        /// E.g.: CART is to be placed ACROSS. It checks valid overlaps with existing DOWN words - ARC, PARTY.
        ///       A   P
        ///       R   A
        ///       C A R T
        ///           T
        ///           Y
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        /// <returns></returns>
        private bool TopCellFreeForRightDirectedWord(int x, int y, string word)
        {
            try
            {
                if (y == 0) return true;
                bool isValid = true;
                if (y - 1 >= 0)
                {
                    for (int i = 0; i < word.Length; x++, i++)
                    {
                        if (Matrix[x, y - 1] != '\0')
                            isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Up);
                        if (!isValid) break;
                    }
                }
                return isValid;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the TopCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method checks if any top-side characters of the word to be placed is a valid overlap from an existing DOWB word on the board.
        /// E.g.: CART is to be placed ACROSS. It checks valid overlaps with existing DOWN words - SCOOP, PARTY.
        ///           P
        ///       S   A
        ///       C A R T
        ///       O   T
        ///       O   Y
        ///       P
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        /// <returns></returns>
        private bool BottomCellFreeForRightDirectedWord(int x, int y, string word)
        {
            try
            {
                if (y == _matrixSize) return true;
                bool isValid = true;
                if (y + 1 < _matrixSize)
                {
                    for (int i = 0; i < word.Length; x++, i++)
                    {
                        if (Matrix[x, y + 1] != '\0')
                            isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Down);
                        if (!isValid) break;
                    }
                }
                return isValid;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the BottomCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// If it is a DWON word, then there should not be any character in the adjacent cell to the bottom of the end of this word.
        /// E.g.: If CAT is to be placed downwards, then the bottom cell should be free:        
        ///   C
        ///   A
        ///   T
        /// |___| C L A S S
        /// </summary>
        /// <param name="x">Intended x-position of the word to be placed</param>
        /// <param name="y">Intended y-position of the word to be placed</param>
        /// <param name="word">The word to be placed. E.g.: CART</param>
        private bool BottomMostBottomCellFreeForDownDirectedWord(int x, int y, string word)
        {
            try
            {
                if (y + word.Length == _matrixSize) return true;
                if (y + word.Length < _matrixSize)
                    return Matrix[x, y + word.Length] == '\0';
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in the BottomMostBottomCellFreeForDownDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method first checks if there is a valid overlap with any existing letter in the same cell of the matrix.
        /// It also makes sure the first few words don't overlap to make them sparse around the matrix.
        /// After the first few scattered words, it makes sure all the subsequent letters are overlapped with existing words.
        /// Then it checks legitimate cross-through with an existing word.
        /// If all passes, then it places the word in the collection.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="word"></param>
        /// <param name="wordMeaning"></param>
        /// <param name="currentWordCount"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        private bool PlaceTheWord(Direction direction, int x, int y, string word, string wordMeaning, int currentWordCount, ref long attempts)
        {
            try
            {
                attempts++;
                bool placeAvailable = true, overlapped = false;
                switch (direction)
                {
                    case Direction.Right:
                        for (int i = 0, xx = x; i < word.Length; i++, xx++) // First we check if the word can be placed in the array. For this it needs blanks there or the same letter (of another word) in the cell.
                        {
                            if (xx >= _matrixSize) return false;  // Falling outside the grid. Hence placement unavailable.
                            if (Matrix[xx, y] != '\0')
                            {
                                if (Matrix[xx, y] != word[i])   // If there is an overlap, then we see if the characters match. If matches, then it can still go there.
                                {
                                    placeAvailable = false;
                                    break;
                                }
                                else overlapped = true;
                            }
                        }

                        if (!placeAvailable)
                            return false;

                        // The first few words should be placed without overlapping.
                        if (currentWordCount < 15 && overlapped)
                            return false;

                        // If overlapping didn't occur after the maximum allowed non-overlapping words for the first few runs (e.g. first 5 words)
                        // then trigger a re-position attempt (by returning false to the calling method which will in turn trigger another search until
                        // an overlapping position is found.)
                        else if (currentWordCount >= 15 && !overlapped)
                            return false;

                        // If it is a right-direction, then there should not be any character in the adjacent cell to the left of the beginning of this word.
                        // E.g.: If CAT is to be placed, then it cannot be placed with another word CLASS as C L A S S C A T
                        bool leftFree = LeftCellFreeForRightDirectedWord(x, y, word);

                        // If it is a right-direction, then check if it can cross through another word. E.g.: if CAT is the current word,
                        // then check if there is a valid crossing through existing words on the board - VICINITY, STICK:
                        // V
                        // I   S
                        // C A T
                        // I   I
                        // N   C
                        // I   K
                        // T
                        // Y
                        bool topFree = TopCellFreeForRightDirectedWord(x, y, word);

                        // If it is a right-direction, then check if it can cross through another word. E.g.: if CAT is the current word,
                        // then check if there is a valid crossing through existing words on the board - VICINITY, STICK:
                        // V
                        // I   S
                        // C A T
                        // I   I
                        // N   C
                        // I   K
                        // T
                        // Y
                        bool bottomFree = BottomCellFreeForRightDirectedWord(x, y, word);

                        // If it is a right-direction, then there should not be any character in the adjacent cell to the right of the ending of this word.
                        // E.g.: If CAT is to be placed, then it cannot be placed with another word BUS as C A T B U S
                        bool rightMostFree = RightMostCellFreeForRightDirectedWord(x, y, word);

                        // If cells that need to be free are not free, then this word cannot be placed there.
                        if (!leftFree || !topFree || !bottomFree || !rightMostFree) return false;

                        // If all the cells are blank, or a non-conflicting overlap is available, then this word can be placed there. So place it.
                        for (int i = 0, j = x; i < word.Length; i++, j++)
                            Matrix[j, y] = word[i];
                        SaveWordDetailsInCollection(word, wordMeaning, x, y, direction, attempts, false);
                        return true;
                    case Direction.Down:
                        for (int i = 0, yy = y; i < word.Length; i++, yy++)     // First we check if the word can be placed in the array. For this it needs blanks there or the same letter (of another word) in the cell.
                        {
                            if (yy >= _matrixSize) return false;      // Falling outside the grid. Hence placement unavailable.
                            if (Matrix[x, yy] != '\0')
                            {
                                if (Matrix[x, yy] != word[i])                   // If there is an overlap, then we see if the characters match. If matches, then it can still go there.
                                {
                                    placeAvailable = false;
                                    break;
                                }
                                else overlapped = true;
                            }
                        }

                        if (!placeAvailable)
                            return false;

                        // The first few words should be placed without overlapping.
                        if (currentWordCount < 15 && overlapped)
                            return false;

                        // If overlapping didn't occur after the maximum allowed non-overlapping words for the first few runs (e.g. first 5 words)
                        // then trigger a re-position attempt (by returning false to the calling method which will in turn trigger another search until
                        // an overlapping position is found.)
                        else if (currentWordCount >= 15 && !overlapped)
                            return false;

                        // If it is a right-direction, then check if it can cross through another word. E.g.: If STICK the current word, see if there
                        // is a valid crossing through existing words on the board - CAT, SKID:
                        //     S
                        // C A T
                        //     I
                        //     C
                        //   S K I D
                        leftFree = LeftCellFreeForDownDirectedWord(x, y, word);

                        // If it is a down-direction, then there should not be any character in the adjacent cell to the right of the beginning of this word.
                        // E.g.: If CAT is to be placed downwards, then it cannot be placed with another word CLASS as
                        // C C L A S S
                        // A
                        // T
                        bool rightFree = RightCellFreeForDownDirectedWord(x, y, word);

                        // If it is a down-direction, then there should not be any character in the adjacent cell to the top of the beginning of this word.
                        // E.g.: If CAT is to be placed downwards, then it cannot be placed with another word CLASS as
                        // C L A S S
                        // C
                        // A
                        // T
                        topFree = TopCellFreeForDownDirectedWord(x, y, word);

                        // If it is a down-direction, then there should not be any character in the adjacent cell to the bottom of the end of this word.
                        // E.g.: If CAT is to be placed downwards, then it cannot be placed with another word CLASS as                        
                        // C
                        // A
                        // T
                        // C L A S S
                        bool bottomMostBottomFree = BottomMostBottomCellFreeForDownDirectedWord(x, y, word);

                        // If cells that need to be free are not free, then this word cannot be placed there.
                        if (!leftFree || !rightFree || !topFree || !bottomMostBottomFree) return false;

                        // If all the cells are blank, or a non-conflicting overlap is available, then this word can be placed there. So place it.
                        for (int i = 0, j = y; i < word.Length; i++, j++)
                            Matrix[x, j] = word[i];
                        SaveWordDetailsInCollection(word, wordMeaning, x, y, direction, attempts, false);
                        return true;
                }
                return false;   // Otherwise continue with a different place and index.
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in 'PlaceTheWords()' method of the 'GameEngine' class. Error msg: {e.Message}");
                return false;   // Otherwise continue with a different place and index.
            }
        }

        /// <summary>
        /// Keeps the word and details in the collection.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="wordMeaning"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        /// <param name="attempts"></param>
        /// <param name="failedMaxAttempts"></param>
        private void SaveWordDetailsInCollection(string word, string wordMeaning, int x, int y, Direction direction, long attempts, bool failedMaxAttempts)
        {
            try
            {
                var details = new RegularWordDetails
                {
                    Word = word,
                    WordMeaning = wordMeaning,
                    X = x,
                    Y = y,
                    WordDirection = direction,
                    AttemptsCount = attempts,
                    FailedMaxAttempts = failedMaxAttempts
                };

                WordDetails.Add(details);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"An error occurred in 'SaveWordDetailsInCollection()' method of the 'GameEngine' class. Error msg: {e.Message}");
            }
        }
    }

    #endregion
}
