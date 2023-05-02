using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview
{
    internal class Interview20230428
    {
        public Interview20230428()
        {
            Console.WriteLine("Welcome to our interview.");
            Console.WriteLine("Complete the following two questions at first.");
            Console.WriteLine("The time is 30 minutes, you can surf the Internet.");
            Tessttt();
            Console.ReadLine();
        }

        /// <summary>
        /// 編寫一個C#函數，接受兩個整數作為輸入，並返回它們的最大公因數。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GCD(int a, int b)
        {
            // 實現該方法
            return 0;
        }


        /// <summary>
        /// 請重構以提高可讀性和性能。
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static void Tessttt()
        {
            int[] intArray = { 1, 2, 3, 4, 5 };
            List<int> intList = intArray.ToList();

            var MyEvenNumbers = new List<int>();
            var MyOddNumbers = new List<int>();

            foreach (var ITEM in intList)
            {
                string numberType;
                if (ITEM % 2 == 0)
                {
                    numberType = "Even";
                    MyEvenNumbers.Add(ITEM);
                }
                else
                {
                    numberType = "Odd";
                    MyOddNumbers.Add(ITEM);
                }
                Console.WriteLine($"{ITEM} is {numberType}");
            }

            Console.WriteLine("Even numbers sum:");
            int even_sum = 0;
            foreach (var even in MyEvenNumbers)
            {
                even_sum += even;
            }
            Console.WriteLine(even_sum);

            Console.WriteLine("Odd numbers sum:");
            int odd_sum = 0;
            foreach (var odd in MyOddNumbers)
            {
                odd_sum += odd;
            }
            Console.WriteLine(odd_sum);
        }
    }
}
