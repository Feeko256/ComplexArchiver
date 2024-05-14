using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ComplexCompressor.Encryptor
{
    public static class FFT_Complex_Encryptor
    {
        // Метод для преобразования массива комплексных чисел обратно в бинарный код
        public static string InverseFourierTransform(Complex[] data)
        {
            FourierTransform(data, FourierTransformDirection.Backward);

            StringBuilder result = new StringBuilder();
            foreach (Complex c in data)
            {
                // Используем пороговую функцию для интерпретации значения комплексного числа как бита (0 или 1)
                double value = (c.Real >= 0.5) ? 1 : 0;
                result.Append(value);
            }

            return result.ToString();
        }

        // Метод для преобразования массива данных с помощью преобразования Фурье
        public static Complex[] FourierTransform(string input)
        {
            int n = input.Length;
            int paddedLength = NextPowerOfTwo(n);
            Complex[] data = new Complex[paddedLength];

            // Заполнение массива данными из бинарного кода
            for (int i = 0; i < n; i++)
            {
                data[i] = (input[i] == '1') ? new Complex(1, 0) : new Complex(0, 0);
            }

            // Применение преобразования Фурье
            FourierTransform(data, FourierTransformDirection.Forward);

            return data;
        }

        // Метод для преобразования массива данных с помощью преобразования Фурье (рекурсивная реализация)
        private static void FourierTransform(Complex[] data, FourierTransformDirection direction)
        {
            int n = data.Length;
            if (n <= 1)
                return;

            Complex[] even = new Complex[n / 2];
            Complex[] odd = new Complex[n / 2];
            for (int i = 0; i < n / 2; i++)
            {
                even[i] = data[2 * i];
                odd[i] = data[2 * i + 1];
            }

            FourierTransform(even, direction);
            FourierTransform(odd, direction);

            double angle = (direction == FourierTransformDirection.Forward) ? -2 * Math.PI / n : 2 * Math.PI / n;
            Complex w = new Complex(1, 0);
            Complex wn = new Complex(Math.Cos(angle), Math.Sin(angle));
            for (int k = 0; k < n / 2; k++)
            {
                data[k] = even[k] + w * odd[k];
                data[k + n / 2] = even[k] - w * odd[k];
                if (direction == FourierTransformDirection.Forward)
                {
                    data[k] /= 2;
                    data[k + n / 2] /= 2;
                }
                w *= wn;
            }
        }

        // Метод для нахождения ближайшей степени двойки, большей или равной заданному числу
        private static int NextPowerOfTwo(int n)
        {
            int power = 1;
            while (power < n)
            {
                power *= 2;
            }
            return power;
        }

        // Перечисление для указания направления преобразования Фурье
        private enum FourierTransformDirection
        {
            Forward,
            Backward
        }
        // Метод для преобразования текста в бинарный код
        public static string StringToBinary(string text)
        {
            StringBuilder binaryStringBuilder = new StringBuilder();

            foreach (char c in text)
            {
                // Преобразуем символ в байт и затем в строку битового представления
                string binaryChar = Convert.ToString(c, 2).PadLeft(8, '0');
                binaryStringBuilder.Append(binaryChar);
            }

            return binaryStringBuilder.ToString();
        }

        // Метод для преобразования бинарного кода в текст
        public static string BinaryToString(string binary)
        {
            if (string.IsNullOrEmpty(binary) || binary.Length % 8 != 0)
            {
                throw new ArgumentException("Строка должна быть непустой и содержать количество символов, кратное 8.");
            }

            StringBuilder textBuilder = new StringBuilder();

            // Разбиваем бинарную строку на блоки по 8 символов
            for (int i = 0; i < binary.Length; i += 8)
            {
                // Выделяем очередной блок из 8 символов
                string binaryChar = binary.Substring(i, 8);
                // Преобразуем его в символ и добавляем к результату
                textBuilder.Append(Convert.ToChar(Convert.ToByte(binaryChar, 2)));
            }

            return textBuilder.ToString();
        }
        // Метод для перевода массива комплексных чисел в строку
        public static string EncryptedToString(Complex[] encrypted)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Complex c in encrypted)
            {
                sb.Append(c.Real + " " + c.Imaginary + " ");
            }
            return sb.ToString();
        }
        // Метод для восстановления массива комплексных чисел из строки
        public static Complex[] StringToEncrypted(string encryptedString)
        {
            string[] parts = encryptedString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Complex[] result = new Complex[parts.Length / 2];
            for (int i = 0; i < parts.Length; i += 2)
            {
                double real = double.Parse(parts[i]);
                double imag = double.Parse(parts[i + 1]);
                result[i / 2] = new Complex(real, imag);
            }
            return result;
        }
    }
}
