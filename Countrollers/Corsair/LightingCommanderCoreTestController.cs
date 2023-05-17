using HidSharp;
using LEDPlayground.Common;
using LEDPlayground.Enums.Corsair;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace LEDPlayground.Countrollers.Corsair
{
    static class LightingCommanderCoreTestConfig
    {
        public const byte ConnectionType = 8;
        public const int FanTypesQL = 6;
    }

    public class LightingCommanderCoreTestController
    {
        static HidStream _stream;
        public LightingCommanderCoreTestController()
        {
            var deviceList = DeviceList.Local;
            //水冷
            //var deviceInfo = deviceList.GetHidDevices(0x1b1c, 0x0C39).FirstOrDefault();
            var deviceInfo = deviceList.GetHidDevices(0x1b1c, 0x0C32).Aggregate((max, x) => (max == null || x.MaxOutputReportLength > max.MaxOutputReportLength) ? x : max); ;

            try
            {
                deviceInfo.TryOpen(out HidStream stream);
                _stream = stream;
                
                SendWhite();
                //SendRGBData(GetPumpLedData("0xFFFFFF"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"無法打開裝置: {ex.Message}");
            }

        }


        private static void SendWhite()
        {
            string s;
            //string s = "00 08 12 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            //Thread.Sleep(200);


            //s = "00 08 02 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            //Thread.Sleep(200);

            //IsHandleOpen(0)
            s = "00 08 09 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            //IsHandleOpen(1)
            s = "00 08 09 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            //IsHandleOpen(2)
            s = "00 08 09 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            //SetProperty(3, 2)
            s = "00 08 01 03 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            //FetchProperty(3)
            //s = "00 08 02 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            //Thread.Sleep(200);

            //OpenHandle("Lighting", Corsair.Endpoints.LightingController)
            s = "00 08 0d 00 22 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);


            //s = "00 08 02 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            //Thread.Sleep(200);


            //s = "00 08 02 12 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            //Thread.Sleep(200);


            //s = "00 08 02 13 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            //Thread.Sleep(200);

            //OpenHandle("Background", this.Endpoints.LedCount_4Pin)
            s = "00 08 0d 01 1e 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            //SetFanType()
            s = "00 08 06 01 11 00 00 00 0d 00 07 01 01 01 06 01 06 01 06 01 06 01 06 01 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            //this.CloseHandle("Background");
            s = "00 08 05 01 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            _stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            //ReadEndpoint("Background", this.Endpoints.FanStates, 0x09)
            s = "00 08 09 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            Thread.Sleep(200);


            s = "00 08 0d 01 1a 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            Thread.Sleep(200);


            s = "00 08 08 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            Thread.Sleep(200);


            s = "00 08 08 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            Thread.Sleep(200);


            s = "00 08 05 01 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            //_stream.Write(s.ToByteArray());
            Thread.Sleep(200);

            while (true)
            {
                s = "00 08 06 00 4a 00 00 00 12 00 ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff ff";
                _stream.Write(s.ToByteArray());
                Thread.Sleep(200);


                s = "00 08 07 00 ff ff 00 ff ff 00 ff ff 00 ff ff 00 ff ff 00 ff ff 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
                _stream.Write(s.ToByteArray());
                Thread.Sleep(200);
            }
        }
    }
}
