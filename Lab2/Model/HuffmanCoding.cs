using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseProjectCodingBase.Model
{
    public class HuffmanCoding
    {
        private Dictionary<char, string> _codes;
        private HuffmanNode _root;

        public Dictionary<char, string> GetHuffmanTable() => _codes;

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            BuildHuffmanTree(input);
            return string.Concat(input.Select(c => _codes[c]));
        }

        public string Decode(string encodedInput)
        {
            var result = new StringBuilder();
            var current = _root;
            foreach (var bit in encodedInput)
            {
                current = bit == '0' ? current.Left : current.Right;
                if (current.IsLeaf())
                {
                    result.Append(current.Symbol);
                    current = _root;
                }
            }
            return result.ToString();
        }

        public double GetEfficiency(string input, string encoded)
        {
            double averageCodeLength = input.Sum(c => _codes[c].Length) / (double)input.Length;
            double originalBitSize = input.Length * 8;
            double encodedBitSize = input.Length * averageCodeLength;
            return (originalBitSize - encodedBitSize) / originalBitSize;
        }

        private void BuildHuffmanTree(string input)
        {
            var frequencies = input.GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            var nodes = frequencies.Select(kv => new HuffmanNode(kv.Key, kv.Value)).ToList();

            while (nodes.Count > 1)
            {
                nodes = nodes.OrderBy(n => n.Frequency)
                             .ThenBy(n => n.Symbol) 
                             .ToList();

                var left = nodes[0];
                var right = nodes[1];
                nodes.RemoveRange(0, 2);

                nodes.Add(new HuffmanNode(left, right));
            }

            _root = nodes[0];
            _codes = new Dictionary<char, string>();
            GenerateCodes(_root, "");
        }

        private void GenerateCodes(HuffmanNode node, string code)
        {
            if (node == null)
                return;

            if (node.IsLeaf())
            {
                _codes[node.Symbol] = code;
            }
            else
            {
                GenerateCodes(node.Left, code + "0");
                GenerateCodes(node.Right, code + "1");
            }
        }

        public Dictionary<char, string> GetHuffmanCodes()
        {
            return _codes ?? new Dictionary<char, string>();
        }
    }

    public class HuffmanNode
    {
        public char Symbol { get; }
        public int Frequency { get; }
        public HuffmanNode Left { get; }
        public HuffmanNode Right { get; }

        public HuffmanNode(char symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }

        public HuffmanNode(HuffmanNode left, HuffmanNode right)
        {
            Left = left;
            Right = right;
            Frequency = left.Frequency + right.Frequency;
            Symbol = left.Symbol < right.Symbol ? left.Symbol : right.Symbol; // Берем минимальный символ
        }

        public bool IsLeaf() => Left == null && Right == null;
    }
}
