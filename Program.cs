using System;
using System.Threading.Tasks;
using LEDPlayground.Countrollers.LianLi;
using LEDPlayground.Countrollers.Nanoleaf;
using LEDPlayground.Countrollers.Philips;

namespace LEDPlayground
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello world");
            //new LianLiUniHubSLV2Controller(0xFFFFFF, 1);
            //new LianLiStrimerLConnectController(0xFFFFFF);
            //await MiniTrianglesStarterKitController.Test();
            new HueBridge();
        }
    }
}