//using System;
//using System.ComponentModel;
//using System.Linq;

//namespace LEDPlayground.Countrollers.Hyte.Keeb
//{
//    internal class Program
//    {
//        public static void Main(string[] args)
//        {
//            var page1 = new Page1(new byte[] { 0x04, 0x21, 0x00, 0x01, 0x01, 0x01, 0x01, 0x00 });
//        }
//    }

//    public class Page1 
//    {
//        public readonly byte Header = 0x04;
//        public byte Cmd { get; set; }

//        public readonly byte PageNum = 0x00;
//        public readonly byte Data = 0x01;
//        public SWBase SW1{ get; set; }
//        public Page1(byte[] source)
//        {
//            if (source.Length != 64)
//            {
//                //throw new Exception("Page初始化不足64個byte");
//            }
//            switch (source[4])
//            { 
//                case (byte)FunctionPage.MousePage:
//                    SW1 = new MousePage(source.Skip(4).Take(4).ToArray());
//                    break;
//            }
//        }
//    }
//    public class SWBase
//    {
//        public byte FunctionPage { get; set; }
//        public byte PageDescription1 { get; set; }
//        public byte PageDescription2 { get; set; }
//        public byte PageDescription3 { get; set; }


//        public SWBase(byte[] source)
//        {
//            if (source.Length != 4)
//            {
//                throw new Exception("SW初始化不足4個byte");
//            }
//            FunctionPage = source[0];
//            PageDescription1 = source[1];
//            PageDescription2 = source[2];
//            PageDescription3 = source[3];
//        }
//    }
//    public class MousePage: SWBase
//    {
//        public FunctionPage FunctionPageEnum { get; set; }
//        public MousePageSection MousePageSection { get; set; }
//        public MousePageAction MousePageAction { get; set; }
//        public byte MovementAmount { get; set; }
//        public MousePage(byte[] soruce) : base(soruce)
//        {
//            FunctionPageEnum = (FunctionPage)Enum.Parse(typeof(FunctionPage), soruce[0].ToString());
//            MousePageSection = (MousePageSection)Enum.Parse(typeof(MousePageSection), soruce[1].ToString());
//            MousePageAction = (MousePageAction)Enum.Parse(typeof(MousePageAction), soruce[2].ToString());
//            MovementAmount = soruce[3];
//        }
//    }

//    public enum FunctionPage
//    {
//        [Description("DefaultPage")]
//        DefaultPage = 0x00,
//        [Description("MousePage")]
//        MousePage = 0x01,
//    }
//    public enum MousePageSection
//    {
//        [Description("LMR45")]
//        LMR45  = 0x01,
//        [Description("Wheel")]
//        Wheel = 0x02,
//    }
//    public enum MousePageAction
//    {
//        [Description("L")]
//        L = 0x01,
//        [Description("R")]
//        R = 0x02,
//    }
//}