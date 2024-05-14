using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexCompressor.Algorythms
{
    [Serializable]
    public class LZ78
    {
        public List<(int, char)> compressed { get; set; }
        public  List<(int, char)> Compress(string input)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            if (compressed == null)
                compressed = new List<(int, char)>();

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
                    if (!string.IsNullOrEmpty(current))
                    {
                        compressed.Add((dictionary[current], nextChar));
                    }
                    else
                    {
                        compressed.Add((0, nextChar));
                    }
                    dictionary[next] = dictSize++;
                    current = "";
                }
            }

            // If the last sequence is not empty, add it to the list
            if (!string.IsNullOrEmpty(current))
            {
                compressed.Add((dictionary[current], (char)0));
            }

            return compressed;
        }

        public string Decompress(List<(int, char)> compressed)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            string result = "";
            int dictSize = 1;

            foreach (var (prefixIndex, nextChar) in compressed)
            {
                string sequence;
                if (prefixIndex == 0)
                {
                    sequence = nextChar.ToString();
                }
                else
                {
                    sequence = dictionary[prefixIndex] + nextChar;
                }
                result += sequence;
                dictionary[dictSize++] = sequence;
            }

            return result;
        }
    }
}
