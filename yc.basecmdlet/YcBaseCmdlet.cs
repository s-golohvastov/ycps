using Grpc.Core;
using Grpc.Net.Client;
using System.Management.Automation;
using yc.auth;
using yc.config;
using static Yandex.Cloud.Organizationmanager.V1.OrganizationService;

namespace yc.basecmdlet
{
    public class YcBaseCmdlet : PSCmdlet
    {

    }

    public class YcBase<TClient> : PSCmdlet
    {
        private string _endpoint;
        private GrpcChannel _grpcChannel;
        private TClient _grpcClient;
        private Metadata _headers;

        public TClient grpcClient { get => _grpcClient; }
        public Metadata headers { get => _headers; }



        public YcBase()
        {
            _headers = new Metadata();
            _headers.Add("Authorization", $"Bearer {AuthCache.Instance.GetAuthHeader()}");
            var endpointId = YcConfig.Instance.Configuration[$"TypeToEndpointsMappings:{typeof(TClient).Name}"];
            
            _endpoint = YcConfig.Instance.Configuration[$"Settings:{ endpointId }"];
            _grpcChannel = GrpcChannel.ForAddress($"https://{_endpoint}");
            _grpcClient = (TClient)Activator.CreateInstance(typeof(TClient), new object[] { _grpcChannel });
        }

    }
}