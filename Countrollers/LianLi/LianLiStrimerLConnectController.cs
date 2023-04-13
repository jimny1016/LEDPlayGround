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

                    // 設置模式
                    byte modeValue = 1;       // 使用自定義模式
                    int speed = 1;           // 速度
                    int brightness = 0;      // 亮度
                    int direction = 0;       // 方向

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
                        SetMode(modeValue, (byte)zoneIndex, speed, brightness, direction);
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
            const int STRIMERLCONNECT_STRIP_COUNT = 12;
            const int zoneSplit = STRIMERLCONNECT_STRIP_COUNT / 2;
            const string ZONE_TYPE_LINEAR = "1";

            List<Zone> zones = new List<Zone>();

            for (int zoneIdx = 0; zoneIdx < zoneSplit; zoneIdx++)
            {
                Zone newZone = new Zone
                {
                    Name = $"24 Pin ATX Strip {zoneIdx}",
                    Type = ZONE_TYPE_LINEAR,
                    LedsMin = 20,
                    LedsMax = 20,
                    LedsCount = 20,
                    MatrixMap = null
                };

                zones.Add(newZone);
            }

            for (int zoneIdx = zoneSplit; zoneIdx < STRIMERLCONNECT_STRIP_COUNT; zoneIdx++)
            {
                Zone newZone = new Zone
                {
                    Name = $"8 Pin GPU Strip {zoneIdx - zoneSplit}",
                    Type = ZONE_TYPE_LINEAR,
                    LedsMin = 27,
                    LedsMax = 27,
                    LedsCount = 27,
                    MatrixMap = null
                };

                zones.Add(newZone);
            }
            return zones;
        }

        public byte[] SetLedsDirect(byte zone, uint[] ledColors, int ledCount)
        {
            const int StrimerLConnectPacketSize = 254;
            const int StrimerLConnectReportId = 224;
            const int StrimerLConnectColourCommand = 48;
            const int StrimerLConnectCommandByte = 1;
            const int StrimerLConnectDataByte = 2;

            byte[] buffer = new byte[StrimerLConnectPacketSize];
            buffer[0] = StrimerLConnectReportId;
            buffer[1] = StrimerLConnectColourCommand;

            buffer[StrimerLConnectCommandByte] |= zone;

            for (int i = 0; i < ledCount; i++)
            {
                int offset = (3 * i) + StrimerLConnectDataByte;

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

        public byte[] SetMode(byte mode, byte zone, int speed, int brightness, int direction)
        {
            const int StrimerLConnectPacketSize = 254;
            const int StrimerLConnectReportId = 224;
            const int StrimerLConnectColourCommand = 16;
            const int StrimerLConnectCommandByte = 1;
            byte[] speed_data = { 0x02, 0x01, 0x00, 0xFE, 0xFF };
            byte[] brightness_data = { 0x08, 0x03, 0x02, 0x01, 0x00 };
            
            byte[] buffer = new byte[StrimerLConnectPacketSize];
            buffer[0] = StrimerLConnectReportId;
            buffer[1] = StrimerLConnectColourCommand;

            buffer[StrimerLConnectCommandByte] |= zone;

            buffer[2] = mode;
            buffer[3] = speed_data[speed];
            buffer[4] = (byte)((direction == 0) ? 1 : 0);
            buffer[5] = brightness_data[brightness];
            _stream.Write(buffer);

            return buffer;
        }
        public byte[] SetApply()
        {
            const int StrimerLConnectPacketSize = 254;
            const int StrimerLConnectReportId = 224;

            byte[] buffer = new byte[StrimerLConnectPacketSize];
            buffer[0] = StrimerLConnectReportId;
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