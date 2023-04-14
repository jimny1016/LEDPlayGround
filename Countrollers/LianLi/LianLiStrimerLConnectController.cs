using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using HidSharp;

namespace LEDPlayground.Countrollers.LianLi
{
    public class LianLiStrimerLConnectController
    {
        static HidStream _stream;
        const int STRIMER_L_CONNECT_PACKET_SIZE = 254;
        const int STRIMER_L_CONNECT_REPORT_ID = 224;
        const int STRIMER_L_CONNECT_COMMAND_BYTE = 1;
        const int STRIMER_L_CONNECT_COLOUR_COMMAND = 48;
        const int STRIMER_L_CONNECT_DATA_BYTE = 2;
        const int STRIMER_L_CONNECT_MODE_COLOUR_COMMAND = 16;

        // 設置模式
        const byte mode = 1;       // 使用自定義模式
        const int speed = 1;           // 速度
        const int brightness = 0;      // 亮度
        const int direction = 0;       // 方向

        readonly byte[] speed_data = { 0x02, 0x01, 0x00, 0xFE, 0xFF };
        readonly byte[] brightness_data = { 0x08, 0x03, 0x02, 0x01, 0x00 };

        public LianLiStrimerLConnectController(uint color)
        {
            var deviceList = DeviceList.Local;
            var deviceInfo = deviceList.GetHidDevices(0x0CF2, 0xA200).FirstOrDefault();

            if (deviceInfo != null)
            {
                Console.WriteLine($"找到裝置: {deviceInfo}");

                try
                {
                    deviceInfo.TryOpen(out HidStream stream);
                    _stream = stream;

                    var zones = GetZones();
                    for(int zoneIndex = 0; zoneIndex < zones.Count(); zoneIndex++)
                    {
                        // 假設每個 LED 都設置為純白色
                        int ledsCount = zones[zoneIndex].LedsCount;
                        uint[] colors = new uint[ledsCount];
                        for (int i = 0; i < ledsCount; i ++)
                        {
                            colors[i] = color;
                        }
                        SetLedsDirect((byte)zoneIndex, colors, ledsCount);
                        SetMode((byte)zoneIndex);
                    }
                    SetApply();
                    Console.WriteLine("顏色已設置為純白色。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"無法打開裝置: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("找不到裝置。");
            }
        }
        public List<Zone> GetZones()
        {
            const int strimerlConnectStripCount = 12;
            const int zoneSplite = strimerlConnectStripCount / 2;
            const string zoneTypeLinear = "1";
            const int pin24LEDCount = 20;
            const int pin8LEDCount = 27;

            List<Zone> zones = new List<Zone>();

            for (int zoneIdx = 0; zoneIdx < zoneSplite; zoneIdx++)
            {
                Zone newZone = new Zone
                {
                    Name = $"24 Pin ATX Strip {zoneIdx}",
                    Type = zoneTypeLinear,
                    LedsMin = pin24LEDCount,
                    LedsMax = pin24LEDCount,
                    LedsCount = pin24LEDCount,
                    MatrixMap = null
                };

                zones.Add(newZone);
            }

            for (int zoneIdx = zoneSplite; zoneIdx < strimerlConnectStripCount; zoneIdx++)
            {
                Zone newZone = new Zone
                {
                    Name = $"8 Pin GPU Strip {zoneIdx - zoneSplite}",
                    Type = zoneTypeLinear,
                    LedsMin = pin8LEDCount,
                    LedsMax = pin8LEDCount,
                    LedsCount = pin8LEDCount,
                    MatrixMap = null
                };

                zones.Add(newZone);
            }
            return zones;
        }

        public byte[] SetLedsDirect(byte zone, uint[] ledColors, int ledCount)
        {
            byte[] buffer = new byte[STRIMER_L_CONNECT_PACKET_SIZE];
            buffer[0] = STRIMER_L_CONNECT_REPORT_ID;
            buffer[1] = STRIMER_L_CONNECT_COLOUR_COMMAND;

            buffer[STRIMER_L_CONNECT_COMMAND_BYTE] |= zone;

            for (int i = 0; i < ledCount; i++)
            {
                int offset = (3 * i) + STRIMER_L_CONNECT_DATA_BYTE;

                buffer[offset] = RGBGetRValue(ledColors[i]); // R
                buffer[offset + 1] = RGBGetBValue(ledColors[i]); // B
                buffer[offset + 2] = RGBGetGValue(ledColors[i]); // G
            }
            _stream.Write(buffer);
            return buffer;
        }
        static byte RGBGetRValue(uint color)
        {
            return (byte)(color & 0xFF);
        }

        static byte RGBGetGValue(uint color)
        {
            return (byte)((color >> 8) & 0xFF);
        }

        static byte RGBGetBValue(uint color)
        {
            return (byte)((color >> 16) & 0xFF);
        }

        public byte[] SetMode(byte zone)
        {            
            byte[] buffer = new byte[STRIMER_L_CONNECT_PACKET_SIZE];
            buffer[0] = STRIMER_L_CONNECT_REPORT_ID;
            buffer[1] = STRIMER_L_CONNECT_MODE_COLOUR_COMMAND;

            buffer[STRIMER_L_CONNECT_COMMAND_BYTE] |= zone;

            buffer[2] = mode;
            buffer[3] = speed_data[speed];
            buffer[4] = (byte)((direction == 0) ? 1 : 0);
            buffer[5] = brightness_data[brightness];
            _stream.Write(buffer);

            return buffer;
        }
        public byte[] SetApply()
        {
            byte[] buffer = new byte[STRIMER_L_CONNECT_PACKET_SIZE];
            buffer[0] = STRIMER_L_CONNECT_REPORT_ID;
            buffer[1] = 0x2C;
            buffer[2] = 0x0F;
            buffer[3] = 0xFF;
            _stream.Write(buffer);
            return buffer;
        }

    }
    public class Zone
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int LedsMin { get; set; }
        public int LedsMax { get; set; }
        public int LedsCount { get; set; }
        public object MatrixMap { get; set; }
    }
}