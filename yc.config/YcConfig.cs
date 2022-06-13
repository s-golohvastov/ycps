using System.Text.Json;
using yc.models;
using Microsoft.Extensions.Configuration;

namespace yc.config
{
    public class YcConfig
    {
        private static Lazy<YcConfig> _instance = new Lazy<YcConfig>(() => new YcConfig());
        public static YcConfig Instance { get { return _instance.Value; } }

        private IConfigurationRoot _configuration;
        public IConfigurationRoot Configuration { get => _configuration; }
        private YcConfig()
        {

            var currentAssemblyparentFolder = Directory.GetParent(System.Reflection.Assembly.GetAssembly(typeof(YcConfig)).Location).FullName;
            _configuration = new ConfigurationBuilder()
                                    .SetBasePath(currentAssemblyparentFolder)
                                    .AddJsonFile("appsettings.json")
                                    .AddUserSecrets<YcConfig>()
                                    .Build();

            EndpointList list = RequestEndpointList(); //try refresh the list
            if (list.endpoints.Length > 0)
            {
                foreach (var item in list.endpoints)
                {
                    _configuration[$"Settings:{item.id}"] = item.address;
                }
            }
            else
            {
                foreach (EndpointRecord item in _configuration.GetSection("endpoints").GetChildren())
                {
                    _configuration[$"Settings:{item.id}"] = item.address;
                }
            }
        }

        private EndpointList RequestEndpointList()
        {
            var client = new HttpClient();
            string result = string.Empty;

            try
            {
                var response = client.GetAsync(_configuration["Settings:YandexAPIEndpoints"]).Result;
                var r = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<EndpointList>(r);
            }
            catch
            {
                return new EndpointList();
            }
        }
    }
}