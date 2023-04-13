using System;
using LEDPlayground.Countrollers.LianLi;

namespace LEDPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");
            //new LianLiUniHubSLV2Controller(0xFFFFFF, 1);
            new LianLiStrimerLConnectController(0xFFFFFF, 1);
        }

    }
}