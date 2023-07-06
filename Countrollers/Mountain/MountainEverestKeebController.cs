using System;
using HidSharp;
using System.Linq;
using System.Collections.Generic;
using LEDPlayground.Common;

namespace LEDPlayground.Countrollers.Mountain
{
    public class MountainEverestKeebController
    {
        private static readonly Dictionary<int, Tuple<int, int>> MID_LAYOUT = new Dictionary<int, Tuple<int, int>>()
        {
            { 0,Tuple.Create(0,4)},
            { 1,Tuple.Create(1,4)},
            { 2,Tuple.Create(2,4)},
            { 3,Tuple.Create(3,4)},
            { 4,Tuple.Create(4,4)},
            { 5,Tuple.Create(5,4)},
            { 6,Tuple.Create(1,0)},
            { 7,Tuple.Create(2,3)},
            { 8,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 9,Tuple.Create(0,6)},
            { 10,Tuple.Create(1,5)},
            { 11,Tuple.Create(2,6)},
            { 12,Tuple.Create(3,6)},
            { 13,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 14,Tuple.Create(5,5)},
            { 15,Tuple.Create(2,3)},
            { 16,Tuple.Create(2,2)},
            { 17,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 18,Tuple.Create(0,7)},
            { 19,Tuple.Create(1,6)},
            { 20,Tuple.Create(2,7)},
            { 21,Tuple.Create(3,7)},
            { 22,Tuple.Create(4,6)},
            { 23,Tuple.Create(5,6)},
            { 24,Tuple.Create(2,1)},
            { 25,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 26,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 27,Tuple.Create(0,8)},
            { 28,Tuple.Create(1,7)},
            { 29,Tuple.Create(2,8)},
            { 30,Tuple.Create(3,8)},
            { 31,Tuple.Create(4,7)},
            { 32,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 33,Tuple.Create(4,3)},
            { 34,Tuple.Create(4,0)},
            { 35,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 36,Tuple.Create(0,9)},
            { 37,Tuple.Create(1,8)},
            { 38,Tuple.Create(2,9)},
            { 39,Tuple.Create(3,9)},
            { 40,Tuple.Create(4,8)},
            { 41,Tuple.Create(5,0)},
            { 42,Tuple.Create(4,1)},
            { 43,Tuple.Create(4,2)},
            { 44,Tuple.Create(3,6)},
            { 45,Tuple.Create(0,11)},
            { 46,Tuple.Create(1,9)},
            { 47,Tuple.Create(2,11)},
            { 48,Tuple.Create(3,11)},
            { 49,Tuple.Create(4,9)},
            { 50,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 51,Tuple.Create(3,0)},
            { 52,Tuple.Create(3,1)},
            { 53,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 54,Tuple.Create(0,12)},
            { 55,Tuple.Create(1,10)},
            { 56,Tuple.Create(2,12)},
            { 57,Tuple.Create(3,12)},
            { 58,Tuple.Create(4,11)},
            { 59,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 60,Tuple.Create(3,2)},
            { 61,Tuple.Create(2,0)},
            { 62,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 63,Tuple.Create(0,13)},
            { 64,Tuple.Create(1,11)},
            { 65,Tuple.Create(2,13)},
            { 66,Tuple.Create(3,13)},
            { 67,Tuple.Create(4,13)},
            { 68,Tuple.Create(5,16)},
            { 69,Tuple.Create(2,1)},
            { 70,Tuple.Create(2,2)},
            { 71,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 72,Tuple.Create(0,14)},
            { 73,Tuple.Create(1,12)},
            { 74,Tuple.Create(2,14)},
            { 75,Tuple.Create(3,14)},
            { 76,Tuple.Create(4,14)},
            { 77,Tuple.Create(5,17)},
            { 78,Tuple.Create(5,0)},
            { 79,Tuple.Create(5,2)},
            { 80,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 81,Tuple.Create(0,16)},
            { 82,Tuple.Create(1,13)},
            { 83,Tuple.Create(2,15)},
            { 84,Tuple.Create(3,15)},
            { 85,Tuple.Create(4,15)},
            { 86,Tuple.Create(5,18)},
            { 87,Tuple.Create(1,18)},
            { 88,Tuple.Create(2,20)},
            { 89,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 90,Tuple.Create(0,17)},
            { 91,Tuple.Create(1,14)},
            { 92,Tuple.Create(2,16)},
            { 93,Tuple.Create(3,16)},
            { 94,Tuple.Create(4,16)},
            { 95,Tuple.Create(5,19)},
            { 96,Tuple.Create(1,20)},
            { 97,Tuple.Create(2,21)},
            { 98,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 99,Tuple.Create(0,18)},
            { 100,Tuple.Create(1,16)},
            { 101,Tuple.Create(2,17)},
            { 102,Tuple.Create(3,17)},
            { 103,Tuple.Create(4,17)},
            { 104,Tuple.Create(5,20)},
            { 105,Tuple.Create(1,21)},
            { 106,Tuple.Create(2,22)},
            { 107,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 108,Tuple.Create(0,19)},
            { 109,Tuple.Create(1,17)},
            { 110,Tuple.Create(2,18)},
            { 111,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 112,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 113,Tuple.Create(5,21)},
            { 114,Tuple.Create(0,21)},
            { 115,Tuple.Create(1,22)},
            { 116,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 117,Tuple.Create(0,20)},
            { 118,Tuple.Create(CONTINUE_ITEM,CONTINUE_ITEM)},
            { 119,Tuple.Create(2,19)},
            { 120,Tuple.Create(3,19)},
            { 121,Tuple.Create(4,18)},
            { 122,Tuple.Create(5,22)},
            { 123,Tuple.Create(0,22)},
            { 124,Tuple.Create(4,21)},
        };
        private const int CONTINUE_ITEM = 99;
        private const int DEVICE_VID = 0x3282;
        private const int DEVICE_PID = 0x0001;
        private const int MAX_FEATURE_LENGTH = 65;
        private const int MAX_SPLIT_BUFFER = 399;
        private const int COMMOND_COUNT = 8;
        private const int GET_BUFFER_SIZE = 57;
        public static List<HidStream> GetHidStreams(int vid, int pid, int maxReportLength, bool isNoneReadWritePermissions = false)
        {
            List<HidStream> hidStreams = null;
            foreach (var device in DeviceList.Local.GetHidDevices(vid, pid).Where(x => x.GetMaxFeatureReportLength() == maxReportLength))
            {
                if (hidStreams == null)
                {
                    hidStreams = new List<HidStream>();
                }
                if (isNoneReadWritePermissions)
                {
                    OpenConfiguration operate = new OpenConfiguration();
                    operate.SetOption(OpenOption.Priority, OpenPriority.High);
                    if (device.TryOpen(operate, out HidStream stream))
                    {
                        hidStreams.Add(stream);
                    }
                }
                else
                {
                    if (device.TryOpen(out HidStream stream))
                    {
                        hidStreams.Add(stream);
                    }
                }
            }

            return hidStreams;
        }
        private static DeviceStream _deviceStream;
        private static HidStream _hidStrem;

        private static void SetCurrentLedEffectOff()
        {
            try
            {
                byte[] sendArray = new byte[MAX_FEATURE_LENGTH];

                sendArray[1] = 0x14;
                sendArray[5] = 0x01;
                sendArray[6] = 0x06;
                _deviceStream.Write(sendArray);
                //sendArray = new byte[MAX_FEATURE_LENGTH] { 0x00, 0x14, 0x2c, 0x0a, 0x00, 0xff, 0x64, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
                sendArray = new byte[MAX_FEATURE_LENGTH];
                sendArray[1] = 0x13;
                sendArray[2] = 0x55;
                sendArray[5] = 0x06;
                _deviceStream.Write(sendArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MountainEverestDevice/SetCurrentLedEffectOff is failed. EX:{ex}.");
            }
        }
        public MountainEverestKeebController()
        {
            List<HidStream> streams = GetHidStreams(DEVICE_VID, DEVICE_PID, MAX_FEATURE_LENGTH);

            _deviceStream = streams[0];
            _hidStrem = streams[0];

            var _displayColorBytes = new List<byte>();
            foreach (var dic in MID_LAYOUT)
            {
                if (dic.Value.Item1 == CONTINUE_ITEM)//stream buffer need to add posistion to send 
                {
                    _displayColorBytes.Add(0);
                    _displayColorBytes.Add(0);
                    _displayColorBytes.Add(0xFF);
                    continue;
                }
                if (dic.Value.Item1 == 5)
                {
                    _displayColorBytes.Add(0x00);
                    _displayColorBytes.Add(0xFF);
                    _displayColorBytes.Add(0x00);
                    continue;
                }

                _displayColorBytes.Add(0xFF);
                _displayColorBytes.Add(0x00);
                _displayColorBytes.Add(0x00);
            }

            //SetCurrentLedEffectOff();

            _displayColorBytes.PadListWithZeros(MAX_SPLIT_BUFFER);

            for (int i = 0; i < COMMOND_COUNT; i++)
            {
                List<byte> commandCollect = new() 
                {
                    0x00,
                    0x14,
                    0x2c,
                    0x00,
                    0x01,
                    Convert.ToByte(i),
                    0x4b,
                    0x00
                };

                int startPosistion = i * GET_BUFFER_SIZE;

                if (startPosistion < MAX_SPLIT_BUFFER)
                {
                    commandCollect.AddRange(_displayColorBytes.GetRange(startPosistion, GET_BUFFER_SIZE));
                }

                commandCollect.PadListWithZeros(MAX_FEATURE_LENGTH);

                try
                {
                    _deviceStream.Write(commandCollect.ToArray());
                    _hidStrem.Read();
                }
                catch
                {
                }
            }
        }
    }
}