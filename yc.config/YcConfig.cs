using System.Text.Json;
using yc.models;
using Microsoft.Extensions.Configuration;

namespace yc.config
{
    public class YcConfig
    {
        private static IConfigurationRoot _configuration = new ConfigurationBuilder()
                                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                    .AddJsonFile("appsettings.json")
                                    .AddUserSecrets<YcConfig>()
                                    .Build();

        public static IConfigurationRoot Configuration { get => _configuration; set => _configuration = value; }

        public static async Task<string> GetEndpointById(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("id must not be null");

            var client = new HttpClient();
            var response = await client.GetAsync( _configuration["Settings:YandexAPIEndpoints"] );

            string result = string.Empty;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var r = await response.Content.ReadAsStringAsync();
                if (null != r)
                {
                    EndpointList? res = JsonSerializer.Deserialize<EndpointList>(r);
                    if (null != res)
                    {
                        result = res.endpoints.Where(x => x.id == id).FirstOrDefault().address;
                    }

                }

            }

            return result;
        }
    }
}