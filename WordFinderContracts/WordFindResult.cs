using System;
using System.Collections.Generic;
using System.Linq;

namespace WordFinderContracts
{
    public class WordFindResult
    {
        public string Word { get; set; }
        public List<WordFindResultDetail> Founds { get; set; }

        public override string ToString()
        {
            var plural = Founds.Count > 1 ? "s" : string.Empty;
            var message = $"Word \"{Word}\" found {Founds.Count} time{plural}: {Environment.NewLine}";
            foreach (var found in Founds)
            {
                message += $"\t - {found}{Environment.NewLine}";
            }

            return message;
        }
    }
}
