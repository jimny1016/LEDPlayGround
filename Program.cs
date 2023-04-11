using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Channels;
using HidSharp;

namespace LEDPlayground
{
    internal class Program
    {
        static HidStream _stream;
        static void Main(string[] args)
        {
            var deviceList = DeviceList.Local;
            var deviceInfo = deviceList.GetHidDevices(3314, 41219).FirstOrDefault();

            if (deviceInfo != null)
            {
                Console.WriteLine($"找到裝置: {deviceInfo}");

                try
                {
                    deviceInfo.TryOpen(out HidStream stream);
                    _stream = stream;
                    uint[] colors = new uint[16];

                    for (int i = 0; i < 16; i++)
                    {
                        colors[i] = 0xFFFFFF;
                    }
                    var lEDData = GetLEDData(colors, 16, 1.0f);

                    SendStartAction(0, lEDData.FanIndex + 1);
                    SendColorData(0, (lEDData.FanIndex + 1) * 16, lEDData.LEDData);
                    SendCommitAction(0, UNIHUB_SLV2_LED_MODE_STATIC_COLOR, UNIHUB_SLV2_LED_SPEED_000, UNIHUB_SLV2_LED_DIRECTION_LTR, UNIHUB_SLV2_LED_BRIGHTNESS_100);
                    Console.WriteLine("風扇顏色已設置為純白色。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"無法打開裝置: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("找不到 Lian Li Uni Hub SL V2 裝置。");
            }
        }

        static public LEDDataAndFans GetLEDData(uint[] colors, int num_colors, float brightness)
        {
            byte[] led_data = new byte[16 * 6 * 3];
            int fan_idx = 0;
            int mod_led_idx;
            int cur_led_idx;

            if (num_colors == 0)
            {
                return null; // Do nothing, channel isn't in use
            }

            for (int led_idx = 0; led_idx < num_colors; led_idx++)
            {
                mod_led_idx = (led_idx % 16);

                if (mod_led_idx == 0 && led_idx != 0)
                {
                    fan_idx++;
                }

                float brightness_scale = brightness * BrightnessLimit(colors[led_idx]);

                // Determine current position of led_data array from colors array
                cur_led_idx = ((mod_led_idx + (fan_idx * 16)) * 3);

                led_data[cur_led_idx + 0] = (byte)(RGBGetRValue(colors[led_idx]) * brightness_scale);
                led_data[cur_led_idx + 1] = (byte)(RGBGetBValue(colors[led_idx]) * brightness_scale);
                led_data[cur_led_idx + 2] = (byte)(RGBGetGValue(colors[led_idx]) * brightness_scale);
            }
            var result = new LEDDataAndFans() { LEDData = led_data, FanIndex = fan_idx };
            return result;
        }
        private const byte UNIHUB_SLV2_TRANSACTION_ID = 0x33;

        static public void SendStartAction(byte channel, int numFans)
        {
            byte[] usbBuf = new byte[64];

            usbBuf[0x00] = UNIHUB_SLV2_TRANSACTION_ID;
            usbBuf[0x01] = 0x10;
            usbBuf[0x02] = 0x60;
            usbBuf[0x03] = ((byte)((channel << 4) + numFans));

            _stream.Write(usbBuf);
            Thread.Sleep(5);
        }

        static public void SendColorData(byte channel, int numLeds, byte[] ledData)
        {
            byte[] usbBuf = new byte[352];

            usbBuf[0x00] = UNIHUB_SLV2_TRANSACTION_ID;
            usbBuf[0x01] = (byte)(0x30 + channel);

            Buffer.BlockCopy(ledData, 0, usbBuf, 0x02, numLeds * 3);

            _stream.Write(usbBuf);
            Thread.Sleep(5);
        }

        static public void SendCommitAction(byte channel, byte effect, byte speed, int direction, int brightness)
        {
            byte[] usbBuf = new byte[64];

            usbBuf[0x00] = UNIHUB_SLV2_TRANSACTION_ID;
            usbBuf[0x01] = (byte)(0x10 + channel);
            usbBuf[0x02] = effect;
            usbBuf[0x03] = speed;
            usbBuf[0x04] = (byte)direction;
            usbBuf[0x05] = (byte)brightness;

            _stream.Write(usbBuf);
            Thread.Sleep(5);
        }

        // 以下是一些額外的函數，你可能需要自行實現這些函數
        static float BrightnessLimit(uint color)
        {
            // TODO: 根據你的需求實現此函數
            return 1f;
        }

        static byte RGBGetRValue(uint color)
        {
            return (byte)((color >> 16) & 0xFF);
        }

        static byte RGBGetGValue(uint color)
        {
            return (byte)((color >> 8) & 0xFF);
        }

        static byte RGBGetBValue(uint color)
        {
            return (byte)(color & 0xFF);
        }

        //void SendStartAction(byte channel, int numOfFans)
        //{
        //    // TODO: 實現此函數以發送開始操作
        //}

        //void SendColorData(byte channel, int numOfLEDs, byte[] ledData)
        //{
        //    // TODO: 實現此函數以發送顏色數據
        //}

        //void SendCommitAction(byte channel, int effect, int speed, int direction, int brightness)
        //{
        //    // TODO: 實現此函數以發送提交操作
        //}
        // 還需定義以下常量
        const int UNIHUB_SLV2_LED_MODE_STATIC_COLOR = 1;
        const int UNIHUB_SLV2_LED_SPEED_000 = 2;
        const int UNIHUB_SLV2_LED_DIRECTION_LTR = 0;
        const int UNIHUB_SLV2_LED_BRIGHTNESS_100 = 0;
    }
}
public class LEDDataAndFans
{
    public byte[] LEDData;
    public int FanIndex;
}