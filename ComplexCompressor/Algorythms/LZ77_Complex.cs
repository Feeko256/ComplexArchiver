using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ComplexCompressor.Algorythms
{
    [Serializable]
    public class LZ77_Complex
    {
       public List<Tuple<int, int, Complex>> compressed;
        public List<Tuple<int, int, Complex>> Compress(string input)
        {
            if(compressed==null)
                compressed = new List<Tuple<int, int, Complex>>();
            // Создаем список кортежей для хранения сжатых данных


            // Задаем размер скользящего окна
            int windowSize = 10;

            // Устанавливаем начальную позицию входной строки
            int position = 0;

            // Проходим по всей входной строке для сжатия данных
            while (position < input.Length)
            {
                // Инициализируем переменные для хранения длины совпадения и индекса совпадения
                int matchLength = 0;
                int matchIndex = -1;

                // Проходим по всем символам в окне сравнения
                for (int i = Math.Max(0, position - windowSize); i < position; i++)
                {
                    // Инициализируем счетчик для подсчета длины совпадения
                    int j = 0;

                    // Проверяем совпадение символов
                    while (position + j < input.Length && input[i + j] == input[position + j])
                    {
                        j++;
                    }

                    // Если текущая длина совпадения больше предыдущей, обновляем значения
                    if (j > matchLength)
                    {
                        matchLength = j;
                        matchIndex = i;
                    }
                }

                // Проверяем, если текущая позиция плюс длина совпадения меньше длины входной строки
                if (position + matchLength < input.Length)
                {
                    // Получаем ASCII код символа, следующего за совпадением
                    int asciiCode = (int)input[position + matchLength];

                    // Задаем длину совпадения в мнимую часть комплексного числа
                    int length = matchLength;

                    // Создаем комплексное число, используя ASCII код и длину совпадения
                    Complex compressedChar = new Complex(asciiCode, length);

                    // Добавляем кортеж (смещение, длина совпадения, комплексное число) в список сжатых данных
                    compressed.Add(Tuple.Create(position - matchIndex, matchLength, compressedChar));

                    // Увеличиваем текущую позицию на длину совпадения плюс один символ
                    position += matchLength + 1;
                }
                else
                {
                    // Если нет совпадений, сохраняем текущий символ как есть с длиной совпадения равной 1

                    // Получаем ASCII код текущего символа
                    int asciiCode = (int)input[position];

                    // Создаем комплексное число с длиной совпадения 1
                    Complex compressedChar = new Complex(asciiCode, 1);

                    // Добавляем кортеж (смещение 0, длина совпадения 0, комплексное число) в список сжатых данных
                    compressed.Add(Tuple.Create(0, 0, compressedChar));

                    // Увеличиваем текущую позицию на один символ
                    position++;
                }
            }

            return compressed;
        }

        public string Decompress(List<Tuple<int, int, Complex>> compressed)
        {
            // Создаем объект StringBuilder для хранения декомпрессированной строки
            StringBuilder decompressed = new StringBuilder();

            // Проходим по всем кортежам в списке сжатых данных
            foreach (var tuple in compressed)
            {
                // Получаем смещение, длину совпадения и комплексное число из кортежа
                int offset = (int)tuple.Item1;
                int length = (int)tuple.Item2;
                Complex compressedChar = tuple.Item3;

                // Проверяем, если смещение и длина совпадения равны нулю, значит это отдельный символ
                if (offset == 0 && length == 0)
                {
                    // Декомпрессия символа из действительной части комплексного числа
                    char decompressedChar = (char)compressedChar.Real;

                    // Добавляем декомпрессированный символ к строке
                    decompressed.Append(decompressedChar);
                }
                else
                {
                    // Восстанавливаем повторяющиеся блоки данных

                    // Находим начальный индекс, с которого начинается повторяющийся блок
                    int startIndex = decompressed.Length - offset;

                    // Проходимся по повторяющемуся блоку и добавляем его символы к строке
                    for (int i = 0; i < length; i++)
                    {
                        if (startIndex + i < decompressed.Length)
                        {
                            decompressed.Append(decompressed[startIndex + i]);
                        }
                        else
                        {
                            break; // Если начальный индекс находится за пределами строки
                        }
                    }

                    // Добавляем символ из действительной части комплексного числа к строке
                    char decompressedChar = (char)compressedChar.Real;
                    decompressed.Append(decompressedChar);
                }
            }

            return decompressed.ToString();
        }
    }
}
