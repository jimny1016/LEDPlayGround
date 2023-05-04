using System;
using Newtonsoft.Json;
using System.Diagnostics;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System.Collections.Generic;
using System.Linq;
using LEDPlayground.Models.Philips;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HueApi.Entertainment.Models;
using HueApi.ColorConverters;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment;

namespace LEDPlayground.Countrollers.Philips
{
    public class HueBridge
    {
        public HueBridge()
        {
            //List<PhilipsConfig> philipsConfigs = new List<PhilipsConfig>();
            //var appConfigManager = new AppConfigManager();
            //AppConfig config = appConfigManager.LoadConfig();
            //if (config.PhilipsConfigs != null)
            //{
            //    foreach (var philipsConfig in config.PhilipsConfigs)
            //    {
            //        var hueBridgeAPIHelper = new HueBridgeAPIHelper(philipsConfig.IPAddress, philipsConfig.UserName, philipsConfig.PSK);
            //        var hueBridgeAPIConfig = hueBridgeAPIHelper.GetConfig();
            //        if (hueBridgeAPIConfig != null)
            //        {
            //            philipsConfigs.Add(philipsConfig);
            //        }
            //    }
            //}

            //var localDevices = new HttpClientManager("https://discovery.meethue.com/").GetAsync<List<HueBridgeDiscoveryResponse>>("");
            //if (localDevices.Any())
            //{
            //    var waitingCheckedIPs = localDevices.Select(x => x.InternalIPAddress).Except(philipsConfigs.Select(x => x.IPAddress));
            //    foreach (var waitingCheckedIP in waitingCheckedIPs)
            //    {
            //        var hueBridgeAPIHelper = new HueBridgeAPIHelper(waitingCheckedIP);
            //        var newUser = hueBridgeAPIHelper.GetNewUser();
            //        philipsConfigs.Add(new PhilipsConfig() { IPAddress = waitingCheckedIP, UserName = newUser.UserName, PSK = newUser.ClientKey });
            //    }
            //}

            //if (philipsConfigs.Any())
            //{
            //    config.PhilipsConfigs = philipsConfigs;
            //    appConfigManager.SaveConfig(config);
            //}

            var hueBridgeIpAddress = "192.168.50.236";
            var hueBridgePort = 2100;
            var udpClient = new UdpClient();
            //udpClient.Connect(hueBridgeIpAddress, hueBridgePort);
            //Console.WriteLine("UdpClient connected to {0}:{1}", hueBridgeIpAddress, hueBridgePort);
            //var udpClientDatagramTransport = new UdpClientDatagramTransport(udpClient);


            //var dtlsClientProtocol = new DtlsClientProtocol(new SecureRandom());

            //// 使用 PSK 初始化 MyTlsClient
            //var hueEntertainmentTlsClient = new HueEntertainmentTlsClient(new MyTlsAuthentication(HexStringToByteArray("09C420A4A449218017566B7B697CB9A0"), "zczfHYB0E3uPdNHeYY86S4yQwMSbLquraluqGdmd"));
            //try
            //{
            //    var dtlsTransport = dtlsClientProtocol.Connect(hueEntertainmentTlsClient, udpClientDatagramTransport);
            //}
            //catch (TlsFatalAlert alert)
            //{
            //    Console.WriteLine("TlsFatalAlert: {0}", alert.AlertDescription);
            //    throw;
            //}
            //BasicTlsPskIdentity pskIdentity = new BasicTlsPskIdentity("zczfHYB0E3uPdNHeYY86S4yQwMSbLquraluqGdmd", HexStringToByteArray("09C420A4A449218017566B7B697CB9A0"));

            //var dtlsClient = new DtlsClient(null!, pskIdentity);

            //DtlsClientProtocol clientProtocol = new DtlsClientProtocol(new SecureRandom());
            //var _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //var response = Task.Run(() => _socket.ConnectAsync(IPAddress.Parse(hueBridgeIpAddress), 2100).ConfigureAwait(false)).Result;
            //var ddd = _socket.Connected;
            //var _udp = new UdpTransport(_socket);

            //var _dtlsTransport = clientProtocol.Connect(dtlsClient, _udp);


            //StreamingHueClient client = new StreamingHueClient(hueBridgeIpAddress, "zczfHYB0E3uPdNHeYY86S4yQwMSbLquraluqGdmd", "09C420A4A449218017566B7B697CB9A0");
            //var all = Task.Run(() => client.LocalHueApi.GetEntertainmentConfigurationsAsync()).Result.Data;
            //var group = all.FirstOrDefault();
            ////Create a streaming group
            ////var entGroup = new StreamingGroup(new List<int>() { 8 ,9});
            //var stream = new StreamingGroup(group.Channels)
            //{
            //    IsForSimulator = false
            //};
            //var result = Task.Run(() => client.ConnectAsync(group.Id, simulator: false));

            ////Start auto updating this entertainment group
            //client.AutoUpdateAsync(stream, new CancellationToken(), 50, onlySendDirtyStates: false);

            //StreamingGroup stream = Task.Run(() => SetupAndReturnGroup()).Result;
            //var entLayer = stream.GetNewLayer(isBaseLayer: true);
            //entLayer.AutoCalculateEffectUpdate(new CancellationToken());
            //CancellationTokenSource cst = new CancellationTokenSource();
            //entLayer.SetState(cst.Token, new RGBColor("FFFFFF"), 1);

            StreamingGroup stream = Task.Run(() => SetupAndReturnGroup()).Result;
            var baseEntLayer = stream.GetNewLayer(isBaseLayer: true);
            baseEntLayer.AutoCalculateEffectUpdate(new CancellationToken());
            CancellationTokenSource cst = new CancellationTokenSource();
            baseEntLayer.SetState(cst.Token, new RGBColor("F00FFF"), 0.75);
            cst = WaitCancelAndNext(cst); 
            string[] color = { "FF0000", "00FF00", "0000FF" };
            int i = 0;
            while (true)
            {
                baseEntLayer.SetState(cst.Token, new RGBColor(color[i % 3]), 0.2);
                cst = WaitCancelAndNext(cst);
                i++;
                Thread.Sleep(50);
            }

            //var groupedByDevice = To2DDeviceGroup(entLayer);

            //if (groupedByDevice.Where(x => x.Count() > 5).Any())
            //{
            //    Console.WriteLine("Knight Rider on Gradient Play Lightstrips");
            //    foreach (var group in groupedByDevice.Where(x => x.Count() > 5))
            //    {
            //        group.To2DGroup().KnightRider(cst.Token);
            //    }

            //    //allLightsOrdered.KnightRider(cst.Token);
            //    cst = WaitCancelAndNext(cst);

            //}




            //string[] color = { "FF0000", "00FF00", "0000FF" };
            //int i = 0;
            //while (true)
            //{
            //    entLayer.SetState(cst.Token, new RGBColor(color[i%2]), 0.5);
            //    cst = WaitCancelAndNext(cst);
            //    i++;
            //    Thread.Sleep(25);
            //}
            //var light1 = entLayer.Where(x => x.Id == 1).FirstOrDefault();
            //var lightsOnRightSide = entLayer.GetRight();

            ////Change the color of the single light
            //light1.SetColor(CancellationToken.None, new RGBColor("FF0000"));

            ////Change the subgroup of lights on the right side
            //lightsOnRightSide.SetColor(CancellationToken.None, new RGBColor("FF0000"));
        }
        private static CancellationTokenSource WaitCancelAndNext(CancellationTokenSource cst)
        {
            cst.Cancel();
            cst = new CancellationTokenSource();
            return cst;
        }
        public static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
        public static async Task<StreamingGroup> SetupAndReturnGroup()
        {
            string ip = "192.168.50.236";
            string key = "zczfHYB0E3uPdNHeYY86S4yQwMSbLquraluqGdmd";
            string entertainmentKey = "09C420A4A449218017566B7B697CB9A0";
            var useSimulator = false;

            //string ip = "127.0.0.1";
            //string key = "aSimulatedUser";
            //string entertainmentKey = "01234567890123456789012345678901";
            //var useSimulator = true;


            //Initialize streaming client
            StreamingHueClient client = new StreamingHueClient(ip, key, entertainmentKey);

            //Get the entertainment group
            var all = await client.LocalHueApi.GetEntertainmentConfigurationsAsync();
            var group = all.Data.LastOrDefault();

            Console.WriteLine($"Using Entertainment Group {group.Id}");

            //Create a streaming group
            var stream = new StreamingGroup(group.Channels);
            stream.IsForSimulator = useSimulator;


            //Connect to the streaming group
            await client.ConnectAsync(group.Id, simulator: useSimulator);

            //Start auto updating this entertainment group
            client.AutoUpdateAsync(stream, new CancellationToken(), 50, onlySendDirtyStates: false);

            //Optional: Check if streaming is currently active
            var entArea = await client.LocalHueApi.GetEntertainmentConfigurationAsync(group.Id);
            Console.WriteLine(entArea.Data.First().Status == HueApi.Models.EntertainmentConfigurationStatus.active ? "Streaming is active" : "Streaming is not active");
            return stream;
        }
    }
    public class HueEntertainmentTlsClient: DefaultTlsClient
    {
        private readonly MyTlsAuthentication _tlsAuthentication;

        public HueEntertainmentTlsClient(MyTlsAuthentication tlsAuthentication)
        {
            _tlsAuthentication = tlsAuthentication;
        }

        public override int[] GetCipherSuites()
        {
            return new int[] { CipherSuite.TLS_PSK_WITH_AES_128_GCM_SHA256 };
        }

        public override TlsAuthentication GetAuthentication()
        {
            return _tlsAuthentication;
        }
        public override void NotifyAlertRaised(byte alertLevel, byte alertDescription, string message, Exception cause)
        {
            Console.WriteLine("Alert raised: level {0}, description {1}, message '{2}', cause {3}", alertLevel, alertDescription, message, cause);
            base.NotifyAlertRaised(alertLevel, alertDescription, message, cause);
        }

        public override void NotifyAlertReceived(byte alertLevel, byte alertDescription)
        {
            Console.WriteLine("Alert received: level {0}, description {1}", alertLevel, alertDescription);
            base.NotifyAlertReceived(alertLevel, alertDescription);
        }

        public override void NotifyHandshakeComplete()
        {
            Console.WriteLine("Handshake complete");
            base.NotifyHandshakeComplete();
        }
    }


    public class MyTlsAuthentication: TlsAuthentication
    {
        private readonly byte[] _psk;
        private readonly string _identity;

        public MyTlsAuthentication(byte[] psk, string identity)
        {
            _psk = psk;
            _identity = identity;
        }

        public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest)
        {
            return new MyPskCredentials(new BasicTlsPskIdentity(_identity, _psk));
        }

        public void NotifyServerCertificate(Certificate serverCertificate)
        {
            Console.WriteLine("Server certificate: {0}", serverCertificate.ToString());
            // Validate server certificate here if necessary.
        }
    }
    public class MyPskIdentity: TlsPskIdentity
    {
        private readonly byte[] _psk;
        private readonly byte[] _identity;

        public MyPskIdentity(byte[] psk, string identity)
        {
            _psk = psk;
            _identity = Encoding.UTF8.GetBytes(identity);
        }

        public byte[] GetPsk()
        {
            return _psk;
        }

        public byte[] GetPskIdentity()
        {
            return _identity;
        }

        public void SkipIdentityHint() { }

        public void NotifyIdentityHint(byte[] psk_identity_hint) { }
    }
    public class MyPskCredentials: TlsCredentials
    {
        private readonly TlsPskIdentity _pskIdentity;

        public Certificate Certificate => null;

        public MyPskCredentials(TlsPskIdentity pskIdentity)
        {
            _pskIdentity = pskIdentity;
        }

        public TlsPskIdentity GetPskIdentity()
        {
            return _pskIdentity;
        }

        public byte[] GetPskEncryptionKey()
        {
            return _pskIdentity.GetPsk();
        }
    }
    public class UdpClientDatagramTransport: DatagramTransport
    {
        private readonly UdpClient _client;

        public UdpClientDatagramTransport(UdpClient client)
        {
            _client = client;
        }

        public int GetReceiveLimit()
        {
            return 1500; // MTU - Maximum Transmission Unit
        }

        public int GetSendLimit()
        {
            return 1500; // MTU - Maximum Transmission Unit
        }

        public int Receive(byte[] buf, int off, int len, int waitMillis)
        {
            if (_client.Available > 0)
            {
                var endPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = _client.Receive(ref endPoint);
                Array.Copy(data, 0, buf, off, data.Length);
                return data.Length;
            }
            return -1;
        }

        public void Send(byte[] buf, int off, int len)
        {
            _client.Send(buf, len);
        }

        public void Close()
        {
            _client.Close();
        }
    }

    internal class HueBridgeAPIHelper
    {
        private readonly HttpClientManager _httpClientManager;
        private string _userName;
        private string _clientKey;

        internal HueBridgeAPIHelper(string iPAddress, string userName = "", string clientKey = null)
        {
            _httpClientManager = new HttpClientManager($"https://{iPAddress}/api/");
            _userName = userName;
            _clientKey = clientKey;
        }

        internal UsernameAndClientKey GetNewUser()
        {
            UsernameAndClientKey result = null;
            try
            {
                var request = JsonConvert.SerializeObject(new HueBridgeNewUsesRequest() { DeviceType = "myNexuxDevice", GenerateClientKey = true });
                var apiResult = _httpClientManager.PostAsync<List<HueBridgeNewUserResponse>>("", request);
                if (apiResult.Any())
                {
                    var usernameAndClientKey = apiResult.FirstOrDefault().Success;
                    result = apiResult.FirstOrDefault().Success;
                    _userName = usernameAndClientKey.UserName;
                    _clientKey = usernameAndClientKey.ClientKey;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"GetNewUser is faile:{ex}");
            }

            return result;
        }

        internal HueBridgeConfigResponse GetConfig()
        {
            HueBridgeConfigResponse result = null;
            try
            {
                result = _httpClientManager.GetAsync<HueBridgeConfigResponse>($"{_userName}/config");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"GetNewUser is faile:{ex}");
            }

            return result;
        }
    }
}