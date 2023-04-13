using System;
using System.Linq;
using System.Threading;
using HidSharp;

namespace LEDPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");
            new LianLiUniHubSLV2Controller(0xFFFFFF, 1);
        }

    }
}