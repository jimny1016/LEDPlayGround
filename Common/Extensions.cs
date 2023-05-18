using System;
using System.Collections.Generic;
using System.Linq;

namespace LEDPlayground.Common
{
    public static class Extensions
    {
        public static void PadListWithEmptyStringsToMultipleOfNumber(this List<byte> inputList, int number)
        {
            int remainder = inputList.Count % number;

            if (remainder != 0)
            {
                int numberOfEmptyStringsToAdd = number - remainder;
                inputList.PadListWithZeros(inputList.Count + numberOfEmptyStringsToAdd);
            }
        }

        public static void PadListWithZeros(this List<byte> inputList, int targetSize)
        {
            int numberOfZeros = targetSize - inputList.Count;

            if (numberOfZeros > 0)
            {
                inputList.AddRange(Enumerable.Repeat((byte)0, numberOfZeros));
            }
        }

        public static byte[] ToByteArray(this string input)
        {
            // Replace double spaces with single spaces
            input = input.Replace("  ", " ");

            // Split the string into substrings
            string[] byteStrings = input.Split(' ');

            // Convert each substring to a byte
            byte[] bytes = byteStrings.Select(s => Convert.ToByte(s, 16)).ToArray();
            return bytes;
        }

        public static byte[] Splice(this byte[] source, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (count < 0 || count > source.Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(count));

            byte[] removedItems = source.Skip(startIndex).Take(count).ToArray();
            byte[] remainingItems = source.Take(startIndex).Concat(source.Skip(startIndex + count)).ToArray();

            Array.Resize(ref source, remainingItems.Length);
            Array.Copy(remainingItems, source, remainingItems.Length);

            return removedItems;
        }
    }
}
