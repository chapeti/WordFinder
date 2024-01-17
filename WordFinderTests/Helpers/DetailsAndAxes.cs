// ***************************************************************************************************************************************
// ****** Mehedi Shams Rony: Dec 2018 ****************************************************************************************************
// ****** Purpose: Class that contains a word's axes on the matrix, and flags of success or isolation. ***********************************
// ***************************************************************************************************************************************
using System.Collections.Generic;
using WordFinderTests;
using WordFinderTests.Helpers;

namespace WordPuzzle.Classes
{
    public class RegularWordDetails 
    {
        public string Word { get; set; }
        public string WordMeaning { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Direction WordDirection { get; set; }
        public long AttemptsCount { get; set; }
        public bool FailedMaxAttempts { get; set; }
        public bool Isolated { get; set; }
        public int OutputSequence { get; set; }                         // For output only.
    }

    public class UnicodeWordDetails 
    {
        public string Word { get; set; }
        public string WordMeaning { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Direction WordDirection { get; set; }
        public long AttemptsCount { get; set; }
        public bool FailedMaxAttempts { get; set; }
        public bool Isolated { get; set; }
        public int OutputSequence { get; set; }                         // For output only.
        public List<string> CompositeUnicodeLetters { get; set; }       // For unicode only.
    }
}