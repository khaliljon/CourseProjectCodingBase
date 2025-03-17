using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProjectCodingBase.Model
{
    public class LZW
    {
        public List<int> Compress(string input)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            List<int> compressed = new List<int>();
            string current = string.Empty;

            foreach (char c in input)
            {
                string combined = current + c;
                if (dictionary.ContainsKey(combined))
                {
                    current = combined;
                }
                else
                {
                    if (!string.IsNullOrEmpty(current) && dictionary.ContainsKey(current))
                    {
                        compressed.Add(dictionary[current]);
                    }
                    dictionary.Add(combined, dictionary.Count);
                    current = c.ToString();
                }
            }

            if (!string.IsNullOrEmpty(current) && dictionary.ContainsKey(current))
            {
                compressed.Add(dictionary[current]);
            }

            return compressed;
        }

        public string Decompress(List<int> compressed)
        {
            if (compressed == null || compressed.Count == 0)
                throw new ArgumentException("Список закодированных данных пуст.");

            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
            {
                dictionary.Add(i, ((char)i).ToString());
            }

            int dictSize = 256;
            string current = dictionary[compressed[0]];
            StringBuilder decompressed = new StringBuilder(current);
            string entry = string.Empty;

            for (int i = 1; i < compressed.Count; i++)
            {
                int code = compressed[i];
                if (dictionary.ContainsKey(code))
                {
                    entry = dictionary[code];
                }
                else if (code == dictSize)
                {
                    entry = current + current[0];
                }
                else
                {
                    throw new Exception($"Ошибка декодирования! Код {code} отсутствует в словаре.");
                }

                decompressed.Append(entry);
                dictionary.Add(dictSize++, current + entry[0]);
                current = entry;
            }

            return decompressed.ToString();
        }
    }
}
