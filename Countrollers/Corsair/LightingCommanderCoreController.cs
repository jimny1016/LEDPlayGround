using HidSharp;
using LEDPlayground.Enums.Corsair;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;

namespace LEDPlayground.Countrollers.Corsair
{
    static class LightingCommanderCoreConfig
    {
        public const byte ConnectionType = 8;
        public const int byteCount = 1280;
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
                //SendRGBData(new byte[] { 255,255,255 });

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
        private static void SendRGBData(byte[] rgbData)
        {
            const int initialHeaderSize = 8;
            const int headerSize = 4;
            int bytesSent = 0;

            if (false)
            {
                rgbData = new byte[] { 0x12, 0x00 }.Concat(rgbData).ToArray();
            }

            int totalBytes = rgbData.Length;
            int initialPacketSize = LightingCommanderCoreConfig.byteCount - initialHeaderSize;

            WriteLighting(rgbData.Length, rgbData.Take(initialPacketSize).ToArray());

            totalBytes -= initialPacketSize;
            bytesSent += initialPacketSize;

            while (totalBytes > 0)
            {
                int bytesToSend = Math.Min(LightingCommanderCoreConfig.byteCount - headerSize, totalBytes);
                StreamLighting(rgbData.Take(bytesToSend).ToArray());

                totalBytes -= bytesToSend;
                bytesSent += bytesToSend;
            }
        }

        private static void WriteLighting(int ledCount, byte[] rgbData)
        {
            var packet = new List<byte>();
            packet.Add(0x00);
            packet.Add(8);
            packet.Add(6);
            packet.Add(0x00);
            packet.Add((byte)(ledCount & 0xFF));
            packet.Add((byte)(ledCount >> 8));
            packet.Add(0x00);
            packet.Add(0x00);
            packet.AddRange(rgbData);

            _stream.Write(packet.ToArray());
        }

        private static void StreamLighting(byte[] rgbData)
        {
            var packet = new List<byte>();
            packet.Add(0x00);
            packet.Add(8);
            packet.Add(7);
            packet.Add(0x00);
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
}
