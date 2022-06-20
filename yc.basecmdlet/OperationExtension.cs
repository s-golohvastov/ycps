using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Yandex.Cloud.Compute.V1;
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


        // TODO: change this to something else. this is to iterate through pages of a list request.
        // potentially we want to have a common for all possible list requests without an extension method
        // not sure however if it is a good idea to inherit generated protobuf code ...
        public static ListImagesResponse GetToEnd(this ListImagesResponse response, ListImagesRequest request, ImageService.ImageServiceClient client, Metadata headers)
        {
            ListImagesResponse ret = new ListImagesResponse();
            ret.MergeFrom(response);
            while (!string.IsNullOrEmpty(ret.NextPageToken))
            {
                request.PageToken = response.NextPageToken;
                var r1 = client.List(request, headers);
                ret.MergeFrom(r1);
                ret.NextPageToken = r1.NextPageToken;
            }

            return ret;
        }
    }
}
