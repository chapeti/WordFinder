using System;
using System.Collections.Generic;

namespace WordFinderContracts
{
    public enum WordFinderDirection
    {
        LeftToRight,
        TopToBottom
    }

    public static class WordFinderDirectionExtensions
    {
        public static string AsString(this WordFinderDirection direction)
        {
            switch (direction)
            {
                case WordFinderDirection.LeftToRight: return "Left to right";
                case WordFinderDirection.TopToBottom: return "Top to bottom";
                default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static IEnumerable<WordFinderDirection> GetAll()
        {
            return (WordFinderDirection[])Enum.GetValues(typeof(WordFinderDirection));
        }

    }




}
