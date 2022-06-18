using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Yandex.Cloud.Operation;
using yc.auth;
using yc.config;

namespace yc.basecmdlet
{
    public static class OperationExtension
    {

        // operation client
        private static GrpcChannel _grpcOperationChannel;
        private static OperationService.OperationServiceClient _grpcOperationClient;
        private static readonly Metadata _headers;


        static OperationExtension()
        {
            var operationEndpoint = YcConfig.Instance.Configuration["Settings:operation"];

            _grpcOperationChannel = GrpcChannel.ForAddress($"https://{operationEndpoint}");
            _grpcOperationClient = new OperationService.OperationServiceClient(_grpcOperationChannel);

            _headers = new Metadata();
            _headers.Add("Authorization", $"Bearer {AuthCache.Instance.GetAuthHeader()}");
        }

        public static async Task<Operation> WaitForCompletion(this Operation operation)
        {
            var operationRequest = new GetOperationRequest { OperationId = operation.Id };

            var opResult = await _grpcOperationClient.GetAsync(operationRequest, _headers);
            while (!opResult.Done)
            {
                opResult = await _grpcOperationClient.GetAsync(operationRequest, _headers);
                await Task.Delay(int.Parse(YcConfig.Instance.Configuration["Settings:defaultPollingInterval"]));
            }

            return operation;
        }
    }
}
