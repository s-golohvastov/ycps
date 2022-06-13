using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using yc.auth;
using yc.basecmdlet;
using yc.models;

namespace yc.connect
{
    [Cmdlet(VerbsCommunications.Connect, "YcAccount")]
    public class ConnectYCAccountCmdlet : YcBaseCmdlet
    {

        [Parameter(Mandatory = true)]
        public string OAuthToken;

        protected override void ProcessRecord()
        {
            HttpClient client = new HttpClient();

            var ycRequestBody = new YandexOauthRequest{ yandexPassportOauthToken = OAuthToken };
            string ycRequestBodyString = JsonSerializer.Serialize(ycRequestBody);
            var body = new StringContent( JsonSerializer.Serialize(ycRequestBody), Encoding.UTF8, "application/json");
            var responce = client.PostAsync("https://iam.api.cloud.yandex.net/iam/v1/tokens", body).Result;
            var res = JsonSerializer.Deserialize<IAMTokenRecord>(responce.Content.ReadAsStringAsync().Result);

            AuthCache.Instance.AddEntry("AuthHeader", res.iamToken, res.expiresAt);
        }
    }
}
