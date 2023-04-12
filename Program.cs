using System;
using System.Linq;
using System.Threading;
using HidSharp;

namespace LEDPlayground
{
    internal class Program
    {
        static HidStream _stream;
        const int UNIHUB_SLV2_LED_MODE_STATIC_COLOR = 1;
        const int UNIHUB_SLV2_LED_SPEED_000 = 2;
        const int UNIHUB_SLV2_LED_DIRECTION_LTR = 0;
        const int UNIHUB_SLV2_LED_BRIGHTNESS_100 = 0;
        const byte UNIHUB_SLV2_TRANSACTION_ID = 224;
        const int ONE_FAN_LED_COUNT = 16;
        const int FOUR_FAN_LED_COUNT = ONE_FAN_LED_COUNT * 4;
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
                    uint[] colors = new uint[FOUR_FAN_LED_COUNT];

                    for (int i = 0; i < FOUR_FAN_LED_COUNT; i++)
                    {
                        colors[i] = 0xFFFFFF;
                    }

                    var lEDData = GetLEDData(colors, FOUR_FAN_LED_COUNT, 1f);

                    SendStartAction(0, lEDData.FanIndex + 1);
                    SendColorData(0, (lEDData.FanIndex + 1) * ONE_FAN_LED_COUNT, lEDData.LEDData);
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
            // LED bright save.
            if (brightness > 1)
            {
                brightness = 1;
            }

            byte[] led_data = new byte[FOUR_FAN_LED_COUNT * 6 * 3];
            int fan_idx = 0;
            int mod_led_idx;
            int cur_led_idx;

            if (num_colors == 0)
            {
                return null; // Do nothing, channel isn't in use
            }

            for (int led_idx = 0; led_idx < num_colors; led_idx++)
            {
                mod_led_idx = (led_idx % ONE_FAN_LED_COUNT);

                if (mod_led_idx == 0 && led_idx != 0)
                {
                    fan_idx++;
                }

                // Determine current position of led_data array from colors array
                cur_led_idx = ((mod_led_idx + (fan_idx * ONE_FAN_LED_COUNT)) * 3);

                led_data[cur_led_idx + 0] = (byte)(RGBGetRValue(colors[led_idx]) * brightness);
                led_data[cur_led_idx + 1] = (byte)(RGBGetBValue(colors[led_idx]) * brightness);
                led_data[cur_led_idx + 2] = (byte)(RGBGetGValue(colors[led_idx]) * brightness);
            }
            var result = new LEDDataAndFans() { LEDData = led_data, FanIndex = fan_idx };
            return result;
        }

        static public void SendStartAction(byte channel, int numFans)
        {
            byte[] usbBuf = new byte[FOUR_FAN_LED_COUNT];

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
            byte[] usbBuf = new byte[FOUR_FAN_LED_COUNT];

            usbBuf[0x00] = UNIHUB_SLV2_TRANSACTION_ID;
            usbBuf[0x01] = (byte)(0x10 + channel);
            usbBuf[0x02] = effect;
            usbBuf[0x03] = speed;
            usbBuf[0x04] = (byte)direction;
            usbBuf[0x05] = (byte)brightness;

            _stream.Write(usbBuf);
            Thread.Sleep(5);
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
    }
}
public class LEDDataAndFans
{
    public byte[] LEDData;
    public int FanIndex;
}