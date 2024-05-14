using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ComplexCompressor.Algorythms
{
    [Serializable]
    public class LZ78_Complex
    {
        public List<(int, Complex)> compressed { get; set; }

        public List<(int, Complex)> Compress(string input)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            if (compressed == null)
                compressed = new List<(int, Complex)>();

            string current = "";
            int dictSize = 1;

            foreach (char nextChar in input)
            {
                string next = current + nextChar;
                if (dictionary.ContainsKey(next))
                {
                    current = next;
                }
                else
                {
                    Complex nextComplex = new Complex(nextChar, 0);
                    if (!string.IsNullOrEmpty(current))
                    {
                        compressed.Add((dictionary[current], nextComplex));
                    }
                    else
                    {
                        compressed.Add((0, nextComplex));
                    }
                    dictionary[next] = dictSize++;
                    current = "";
                }
            }

            // If the last sequence is not empty, add it to the list
            if (!string.IsNullOrEmpty(current))
            {
                compressed.Add((dictionary[current], new Complex(0, 0))); // Using (0, 0) to denote end
            }

            return compressed;
        }

        public string Decompress(List<(int, Complex)> compressed)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            string result = "";
            int dictSize = 1;

            foreach (var (prefixIndex, nextComplex) in compressed)
            {
                string sequence;
                if (prefixIndex == 0)
                {
                    sequence = ((char)nextComplex.Real).ToString();
                }
                else
                {
                    sequence = dictionary[prefixIndex] + (char)nextComplex.Real;
                }
                result += sequence;
                dictionary[dictSize++] = sequence;
            }

            return result;
        }
    }
}
