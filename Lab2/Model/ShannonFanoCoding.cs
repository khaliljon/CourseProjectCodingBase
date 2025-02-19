using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseProjectCodingBase.Model
{
    public class ShannonFanoCoding
    {
        private Dictionary<char, string> _codes = new Dictionary<char, string>();
        private Dictionary<string, char> _decodeMap = new Dictionary<string, char>();

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            var frequencies = input.GroupBy(c => c)
                                   .Select(g => new SymbolFrequency(g.Key, g.Count()))
                                   .OrderByDescending(g => g.Frequency)
                                   .ToList();

            GenerateCodes(frequencies, 0, frequencies.Count - 1, "");

            StringBuilder encodedText = new StringBuilder();
            foreach (char c in input)
            {
                encodedText.Append(_codes[c]);
            }

            return encodedText.ToString();
        }

        private void GenerateCodes(List<SymbolFrequency> frequencies, int start, int end, string code)
        {
            if (start == end)
            {
                _codes[frequencies[start].Symbol] = code;
                _decodeMap[code] = frequencies[start].Symbol;
                return;
            }

            int totalFrequency = frequencies.Skip(start).Take(end - start + 1).Sum(f => f.Frequency);
            int cumulativeFrequency = 0;
            int splitIndex = start;

            for (; splitIndex < end; splitIndex++)
            {
                cumulativeFrequency += frequencies[splitIndex].Frequency;
                if (cumulativeFrequency >= totalFrequency / 2)
                    break;
            }

            GenerateCodes(frequencies, start, splitIndex, code + "0");
            GenerateCodes(frequencies, splitIndex + 1, end, code + "1");
        }

        public string Decode(string encodedText)
        {
            StringBuilder decodedText = new StringBuilder();
            string currentCode = "";

            foreach (char bit in encodedText)
            {
                currentCode += bit;
                if (_decodeMap.ContainsKey(currentCode))
                {
                    decodedText.Append(_decodeMap[currentCode]);
                    currentCode = "";
                }
            }

            return decodedText.ToString();
        }

        public double GetCompressionRatio(string input, string encoded)
        {
            int originalSize = input.Length * 8;
            int encodedSize = encoded.Length;
            return (double)encodedSize / originalSize;
        }

        private class SymbolFrequency
        {
            public char Symbol { get; }
            public int Frequency { get; }

            public SymbolFrequency(char symbol, int frequency)
            {
                Symbol = symbol;
                Frequency = frequency;
            }
        }
    }
}