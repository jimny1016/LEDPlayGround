using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Management;
using AuraServiceLib;
using System.Xml.Linq;
using LEDPlayground.Countrollers.ASUS;
using CUESDK;
using HidSharp;
using System.Linq;
using System.Threading.Channels;
using System.Drawing;
using System.Collections.Generic;
using LEDPlayground.Countrollers.Corsair;
using LEDPlayground.Common;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;

namespace LEDPlayground
{
    internal class Program
    {
        private static Socket clientSocket;
        public static void Main(string[] args)
        {
            Console.WriteLine("歡迎來到 Q60 LCD 測試軟件，請確保 Q60 端已打開測試 APP 後輸入任意鍵繼續。");
            Console.WriteLine("請輸入任意鍵繼續:");
            Console.ReadLine();
            try
            {
                Console.WriteLine("開始TCP Socket連線...");
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.42.169"), 8080));
                if(clientSocket == null && !clientSocket.Connected) 
                {
                    throw new Exception("TCP連線失敗，請檢察測試APP是否開啟，或是開啟後裝置的USB連線模式是否有切換到網路共享。");
                }
                Console.WriteLine("TCP Socket連線成功!");
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"TCP連線發生錯誤 ex:{ex}");
                Console.ReadLine();
                throw;
            }

            while (true)
            {
                Console.WriteLine("請輸入指令，0:切換控制節點至安卓，1:切換控制節點至STM32，2:切換螢幕黑白，3:進入顏色測試模式");
                var command = Console.ReadLine();
                switch(command)
                {
                    case "0":
                        Console.WriteLine("切換控制節點至安卓中...");
                        SendCommand("SwitchToAndroidControllMode");
                        Console.WriteLine("切換控制節點至安卓成功!");
                        break;
                    case "1":
                        Console.WriteLine("切換控制節點至STM32中...");
                        SendCommand("SwitchToSTM32ControllMode");
                        Console.WriteLine("切換控制節點至STM32成功!");
                        break;
                    case "2":
                        break;
                    default:
                        Console.WriteLine("指令不符合規範。");
                        break;                    
                }
            }
        }
        private static void SendCommand(string socketCommand)
        {
            try
            {
                byte[] lengthBytes = Encoding.UTF8.GetBytes(socketCommand.Length.ToString());

                // 傳送資料長度
                clientSocket.Send(lengthBytes);

                byte[] pong = new byte[7];
                // 接收對方回傳的資料長度
                clientSocket.Receive(pong);

                // 確認回傳的資料是否為current
                var receivedLength = Encoding.UTF8.GetString(pong);
                if (receivedLength != "current")
                {
                    throw new Exception("傳送資料長度異常。");
                }

                byte[] commandBytes = Encoding.UTF8.GetBytes(socketCommand);
                // 傳送資料
                clientSocket.Send(commandBytes);

                // 接收對方回傳的確認資料
                clientSocket.Receive(pong);

                // 確認回傳的確認資料是否為current
                var receivedConfirmation = Encoding.UTF8.GetString(pong);
                if (receivedConfirmation != "current")
                {
                    throw new Exception("傳送資料異常。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"傳送資料異常 ex:{ex}");
            }
        }
    }
}