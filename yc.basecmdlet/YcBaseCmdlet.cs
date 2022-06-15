using Grpc.Core;
using Grpc.Net.Client;
using System.Management.Automation;
using System.Runtime.InteropServices;
using Yandex.Cloud.Operation;
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

        // operation client
        private GrpcChannel _grpcOperationChannel;
        private OperationService.OperationServiceClient _grpcOperationClient;

        public TClient grpcClient { get => _grpcClient; }

        public OperationService.OperationServiceClient grpcOperationClient { get => _grpcOperationClient; }
        public Metadata headers { get => _headers; }



        public YcBase()
        {
            _headers = new Metadata();
            _headers.Add("Authorization", $"Bearer {AuthCache.Instance.GetAuthHeader()}");
            var endpointId = YcConfig.Instance.Configuration[$"TypeToEndpointsMappings:{typeof(TClient).Name}"];
            var operationEndpoint = YcConfig.Instance.Configuration["Settings:operation"];

            if (string.IsNullOrEmpty(endpointId))
            {
                var ex = new Exception($"cannot resolve API endpoint from the {typeof(TClient).Name} type");
                var err = new ErrorRecord(ex, "100", ErrorCategory.MetadataError, null);
                ThrowTerminatingError(err);
            }

            _endpoint = YcConfig.Instance.Configuration[$"Settings:{ endpointId }"];
            _grpcChannel = GrpcChannel.ForAddress($"https://{_endpoint}");
            _grpcClient = (TClient)Activator.CreateInstance(typeof(TClient), new object[] { _grpcChannel });

            _grpcOperationChannel = GrpcChannel.ForAddress($"https://{operationEndpoint}");
            _grpcOperationClient = (OperationService.OperationServiceClient)Activator.CreateInstance(typeof(OperationService.OperationServiceClient), new object[] { _grpcOperationChannel });
        }

    }
}