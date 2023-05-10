using AuraServiceLib;
using System;

namespace LEDPlayground.Countrollers.ASUS
{
    public class Z790Controller
    {
        public Z790Controller()
        {
            // Create SDK instance
            IAuraSdk sdk = new AuraSdk();// Aquire control
            sdk.SwitchMode();

            // enumerate all devices
            IAuraSyncDeviceCollection devices = sdk.Enumerate(0);
            foreach (IAuraSyncDevice dev in devices)
            {
                Console.WriteLine($"I'm device:{dev.Name}");
                Console.WriteLine($"I'm device:{dev.Type}");
                //if (dev.Lights.Count > 0) { Console.WriteLine($"I'm device:{dev.Name}"); Console.WriteLine($"I'm device:{dev.Type}"); }

                //if (dev.Name != "AddressableStrip 3")
                //    continue;
                // Traverse all LED's
                //foreach (IAuraRgbLight light in dev.Lights)
                //{
                //    //if(light.Name != "RGB HEADER")
                //    //    continue;
                //    Console.WriteLine($"I'm light:{light.Name}");
                //    light.Color = 0x00FF0000;//0x00BBGGRR
                //}
                //dev.Apply();
            }
        }
    }
}
