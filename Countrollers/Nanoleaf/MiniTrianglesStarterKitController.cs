using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace LEDPlayground.Countrollers.Nanoleaf
{
    public class MiniTrianglesStarterKitController
    {
        private static readonly string NanoleafIP = "192.168.0.88";
        private static readonly string NanoleafKey = "ZS8z5Gir4FsUSZ0PM3GVASOAiDKpUVov";
        public static async Task Test()
        {
            await CreateAndApplyWhitePulseEffectAsync(NanoleafIP, NanoleafKey);
        }

        private static async Task CreateAndApplyWhitePulseEffectAsync(string ip, string key)
        {
            using var httpClient = new HttpClient();

            // 使用 API 密鑰建立授權
            httpClient.DefaultRequestHeaders.Add("Authorization", key);

            // 設置純白色 100 毫秒從亮度 100 到 0 循環的特效 JSON 請求內容
            var effect = new
            {
                write = new
                {
                    command = "display",
                    version = "1.0",
                    animType = "custom",
                    animData = "1 6141 1 100 100 100 0 100", // 每個動畫數據字段解釋：[Panel ID] [R] [G] [B] [亮度(0-100)] [持續時間(毫秒)] [延遲(毫秒)]
                    loop = false,
                    palette = new object[] { }
                }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(effect),
                Encoding.UTF8,
                "application/json");

            // 發送 PUT 請求以設置 Light Panels 的特效
            var response = await httpClient.PutAsync($"http://{ip}:16021/api/v1/{key}/effects", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("已創建並應用純白色 100 毫秒從亮度 100 到 0 循環特效。");
            }
            else
            {
                Console.WriteLine($"創建或應用特效時出錯。 狀態碼：{response.StatusCode}");
            }
        }
    }
}