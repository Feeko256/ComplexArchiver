using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ComplexCompressor.Algorythms
{
    [Serializable]
    public class LZ77
    {
        public List<(int, int, char)> Compressed { get; set; }
        public List<(int Offset, int Length, char NextChar)> Compress(string input, int windowSize, int bufferSize)
        {
            if(Compressed == null)
                Compressed = new List<(int, int, char)> ();
            
            int index = 0;

            while (index < input.Length)
            {
                int matchLength = 0;
                int matchOffset = 0;
                int end = Math.Min(index + bufferSize, input.Length);

                for (int startIndex = Math.Max(0, index - windowSize); startIndex < index; startIndex++)
                {
                    int length = 0;
                    while (length < bufferSize && index + length < input.Length && input[startIndex + length] == input[index + length])
                        length++;

                    if (length > matchLength)
                    {
                        matchLength = length;
                        matchOffset = index - startIndex;
                    }
                }

                char nextChar = index + matchLength < input.Length ? input[index + matchLength] : (char)0;
                Compressed.Add((matchOffset, matchLength, nextChar));
                index += matchLength + 1;
            }

            return Compressed;
        }

        public string Decompress(List<(int Offset, int Length, char NextChar)> compressed)
        {
            List<char> decompressed = new List<char>();

            foreach (var (Offset, Length, NextChar) in compressed)
            {
                int start = decompressed.Count - Offset;
                for (int i = 0; i < Length; i++)
                {
                    decompressed.Add(decompressed[start + i]);
                }
                if (NextChar != 0)
                    decompressed.Add(NextChar);
            }

            return new string(decompressed.ToArray());
        }
    }
}
