using HidSharp;
using HueApi.Models;
using LEDPlayground.Common;
using LEDPlayground.Enums.Corsair;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace LEDPlayground.Countrollers.Corsair
{
    static class LightingCommanderCoreConfig
    {
        public const byte ConnectionType = 8;
        public const int FanTypesQL = 6;
    }

    public class LightingCommanderCoreController
    {
        static HidStream _stream;
        public LightingCommanderCoreController()
        {
            var deviceList = DeviceList.Local;
            //水冷
            //var deviceInfo = deviceList.GetHidDevices(0x1b1c, 0x0C39).FirstOrDefault();
            var deviceInfo = deviceList.GetHidDevices(0x1b1c, 0x0C32).FirstOrDefault();

            try
            {
                deviceInfo.TryOpen(out HidStream stream);
                _stream = stream;
                var handler = new LightingCommanderCoreHandle(stream);
                handler.CloseAllHandle();
                SetProperty();
                handler.OpenHandle(Handles.Lighting, Endpoints.LightingController);
                SetFanType();
                handler.OpenHandle(Handles.Background, Endpoints.LedCount_4Pin);
                handler.CloseHandle(Handles.Background);
                SendRGBData(GetPumpLedData("0xFFFFFF"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"無法打開裝置: {ex.Message}");
            }

        }

        public static void SetProperty()
        {
            var packet = new List<byte>
            {
                0x00,
                LightingCommanderCoreConfig.ConnectionType,
                (byte)CommandIds.SetProperty,
                (byte)PropertyNames.Mode,
                0x00,
                (byte)(2 & 0xFF),
                (byte)((2 >> 8) & 0xFF),
                (byte)((2 >> 16) & 0xFF)
            };
            _stream.Write(packet.ToArray());
        }
        public void SetFanType()
        {
            // Configure Fan Ports to use QL Fan size grouping. 34 Leds
            List<byte> fanSettings = Enumerable.Repeat((byte)0, 25).ToList();
            fanSettings[0] = 0x00;
            fanSettings[1] = LightingCommanderCoreConfig.ConnectionType;
            fanSettings[2] = 0x06;
            fanSettings[3] = 0x01;
            fanSettings[4] = 0x11;
            fanSettings[8] = 0x0D;
            fanSettings[10] = 0x07;
            int offset = 11;

            for (int iIdx = 0; iIdx < 7; iIdx++)
            {
                fanSettings[offset + iIdx * 2] = 0x01;
                fanSettings[offset + iIdx * 2 + 1] = (byte)(iIdx == 0 ? 0x01 : LightingCommanderCoreConfig.FanTypesQL); // 1 for nothing, 0x08 for pump?
            }
            _stream.Write(fanSettings.ToArray());

        }
        public static byte[] GetPumpLedData(string color)
        {
            List<byte> RGBData = new List<byte>();

            for (int iIdx = 0; iIdx < EliteLCDCooler.Mapping.Count; iIdx++)
            {
                List<byte> mxPxColor;

                //find colors
                mxPxColor = HexToRgb(color);

                //set colors
                RGBData.Add(mxPxColor[0]);
                RGBData.Add(mxPxColor[1]);
                RGBData.Add(mxPxColor[2]);
            }

            return RGBData.ToArray();
        }

        public static List<byte> HexToRgb(string hex)
        {
            hex = hex.Replace("0x", "");
            var rgb = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                rgb.Add(byte.Parse(hex.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber));
            }

            return rgb;
        }

        private static void SendRGBData(byte[] rgbData)
        {
            const int initialHeaderSize = 8;
            const int headerSize = 4;
            int bytesSent = 0;

            //rgbData = new byte[] { 0x12, 0x00 }.Concat(rgbData).ToArray();

            int totalBytes = rgbData.Length;
            var byteCount = FindBufferLength();
            int initialPacketSize = byteCount - initialHeaderSize;

            WriteLighting(rgbData.Length, rgbData.Take(initialPacketSize).ToArray());

            totalBytes -= initialPacketSize;
            bytesSent += initialPacketSize;

            while (totalBytes > 0)
            {
                int bytesToSend = Math.Min(byteCount - headerSize, totalBytes);
                StreamLighting(rgbData.Take(bytesToSend).ToArray());

                totalBytes -= bytesToSend;
                bytesSent += bytesToSend;
            }
        }

        private static int FindBufferLength()
        {
            var packet = new List<byte>
            {
                0x00,
                LightingCommanderCoreConfig.ConnectionType,
                (byte)CommandIds.PingDevice,
            };

            _stream.Write(packet.ToArray());
            var result = _stream.Read();
            return result.Length;
        }

        private static void WriteLighting(int ledCount, byte[] rgbData)
        {
            var packet = new List<byte>
            {
                0x00,
                LightingCommanderCoreConfig.ConnectionType,
                6,
                0x00,
                (byte)(ledCount & 0xFF),
                (byte)(ledCount >> 8),
                0x00,
                0x00
            };
            packet.AddRange(rgbData);

            _stream.Write(packet.ToArray());
        }

        private static void StreamLighting(byte[] rgbData)
        {
            var packet = new List<byte>
            {
                0x00,
                LightingCommanderCoreConfig.ConnectionType,
                7,
                0x00
            };
            packet.AddRange(rgbData);

            _stream.Write(packet.ToArray());
        }
    }

    public class LightingCommanderCoreHandle
    {
        static HidStream _stream;

        public LightingCommanderCoreHandle(HidStream stream)
        {
            _stream = stream;
        }

        public void CloseAllHandle()
        {
            foreach (Handles handle in Enum.GetValues(typeof(Handles)))
            {
                if (IsHandleOpen(handle))
                {
                    CloseHandle(handle);
                }
            }
        }

        public bool IsHandleOpen(Handles handle)
        {
            byte[] packet = new byte[] { 0x00, LightingCommanderCoreConfig.ConnectionType, (byte)CommandIds.CheckHandle, (byte)handle, 0x00 };

            _stream.Write(packet.ToArray());
            packet = _stream.Read();

            bool isOpen = packet[3] != 3;
            return isOpen;
        }

        public void CloseHandle(Handles handle)
        {
            var packet = new List<byte>
            {
                0x00,
                LightingCommanderCoreConfig.ConnectionType,
                (byte)CommandIds.CloseHandle,
                1,
                (byte)handle
            };
            _stream.Write(packet.ToArray());
        }

        public void OpenHandle(Handles handle, Endpoints endpoint)
        {
            var packet = new List<byte>
            {
                0x00,
                LightingCommanderCoreConfig.ConnectionType,
                (byte)CommandIds.OpenEndpoint,
                (byte)handle,
                (byte)endpoint
            };
            _stream.Write(packet.ToArray());
        }
    }

    public static class EliteLCDCooler
    {
        public static List<List<int>> Positioning { get; } = new List<List<int>>
        {
            new List<int>{6, 0},
            new List<int>{5, 1},
            new List<int>{7, 1},
            new List<int>{4, 2},
            new List<int>{8, 2},
            new List<int>{3, 3},
            new List<int>{9, 3},
            new List<int>{2, 4},
            new List<int>{10, 4},
            new List<int>{1, 5},
            new List<int>{11, 5},
            new List<int>{0, 6},
            new List<int>{12, 6},
            new List<int>{1, 7},
            new List<int>{11, 7},
            new List<int>{2, 8},
            new List<int>{10, 8},
            new List<int>{4, 10},
            new List<int>{8, 10},
            new List<int>{3, 9},
            new List<int>{9, 9},
            new List<int>{5, 11},
            new List<int>{7, 11},
            new List<int>{6, 12}
        };

        public static List<int> Mapping { get; } = new List<int>
        {
            6,
            5, 7,
            4, 8,
            3, 9,
            2, 10,
            1, 11,
            0, 12,
            23, 13,
            22, 14,
            21, 15,
            20, 16,
            19, 17,
            18
        };

        public static List<string> LedNames { get; } = new List<string>
        {
            "Led 1", "Led 2", "Led 3", "Led 4", "Led 5", "Led 6", "Led 7", "Led 8", "Led 9", "Led 10", "Led 11", "Led 12", "Led 13", "Led 14", "Led 15", "Led 16",
            "Led 17", "Led 18", "Led 19", "Led 20", "Led 21", "Led 22", "Led 23", "Led 24"
        };

        public static string DisplayName { get; } = "Elite LCD Cooler";
        public static int LedCount { get; } = 24;
        public static int Width { get; } = 13;
        public static int Height { get; } = 13;
    }
}
