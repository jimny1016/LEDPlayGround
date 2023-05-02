using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using HidSharp.Utility;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System.Collections.Generic;
using System.Linq;
using LEDPlayground.Models.Philips;

namespace LEDPlayground.Countrollers.Nanoleaf
{
    public class HueBridge
    {
        public HueBridge()
        {
            List<PhilipsConfig> philipsConfigs = new List<PhilipsConfig>();
            var appConfigManager = new AppConfigManager();
            AppConfig config = appConfigManager.LoadConfig();
            if (config.PhilipsConfigs != null)
            {
                foreach (var philipsConfig in config.PhilipsConfigs)
                {
                    var hueBridgeAPIHelper = new HueBridgeAPIHelper(philipsConfig.IPAddress, philipsConfig.UserName, philipsConfig.PSK);
                    var hueBridgeAPIConfig = hueBridgeAPIHelper.GetConfig();
                    if (hueBridgeAPIConfig != null)
                    {
                        philipsConfigs.Add(philipsConfig);
                    }
                }
            }

            var localDevices = new HttpClientManager("https://discovery.meethue.com/").GetAsync<List<HueBridgeDiscoveryResponse>>("");
            if (localDevices.Any())
            {
                var waitingCheckedIPs = localDevices.Select(x => x.InternalIPAddress).Except(philipsConfigs.Select(x => x.IPAddress));
                foreach (var waitingCheckedIP in waitingCheckedIPs)
                {
                    var hueBridgeAPIHelper = new HueBridgeAPIHelper(waitingCheckedIP);
                    var newUser = hueBridgeAPIHelper.GetNewUser();
                    philipsConfigs.Add(new PhilipsConfig() { IPAddress = waitingCheckedIP, UserName = newUser.UserName, PSK = newUser.ClientKey });
                }
            }

            if (philipsConfigs.Any())
            {
                config.PhilipsConfigs = philipsConfigs;
                appConfigManager.SaveConfig(config);
            }

            //var hueBridgeIpAddress = "192.168.50.236";
            //var hueBridgePort = 443;
            //var udpClient = new UdpClient();
            //udpClient.Connect(hueBridgeIpAddress, hueBridgePort);
            //var hueEntertainmentTlsClient = new HueEntertainmentTlsClient(psk);
            //var dtlsClientProtocol = new TlsClientProtocol(udpClient.GetStream(), new SecureRandom());
            //dtlsClientProtocol.Connect(hueEntertainmentTlsClient);

        }
    }
    //public class HueEntertainmentTlsClient: DefaultTlsClient
    //{
    //    private readonly byte[] _psk;

    //    public HueEntertainmentTlsClient(byte[] psk)
    //    {
    //        _psk = psk;
    //    }

    //    public override TlsAuthentication GetAuthentication()
    //    {
    //        return new MyTlsAuthentication(_psk);
    //    }
    //}


    //public class MyTlsAuthentication: TlsAuthentication
    //{
    //    private readonly byte[] _psk;

    //    public MyTlsAuthentication(byte[] psk)
    //    {
    //        _psk = psk;
    //    }

    //    public override TlsCredentials GetClientCredentials(CertificateRequest certificateRequest)
    //    {
    //        return new PskTlsClientCredentials(null, _psk);
    //    }

    //    public override void NotifyServerCertificate(Certificate serverCertificate)
    //    {
    //        // Validate server certificate here if necessary.
    //    }
    //}

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