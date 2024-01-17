namespace WordFinderContracts
{
    public class WordFindResultDetail
    {
        public WordFinderDirection Direction { get; set; }
        public int[] From { get; set; }
        public int[] To { get; set; }

        public override string ToString()
        {
            return $"{Direction.AsString()}, from: [{From[0]},{From[1]}] to [{To[0]},{To[1]}]. ";
        }
    }
}
