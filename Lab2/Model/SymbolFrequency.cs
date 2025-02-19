namespace CourseProjectCodingBase
{
    public class SymbolFrequency
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }

        public SymbolFrequency(char symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }
    }
}
