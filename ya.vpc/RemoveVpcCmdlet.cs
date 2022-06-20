using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Yandex.Cloud.Vpc.V1;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Vpc.V1.NetworkService;


namespace yc.vpc
{
    
    // TODO: add -Force flag ot recursively delete subnets (and potentially other nested resources)
    [Cmdlet(VerbsCommon.Remove, "YcVpc")]
    public class RemoveVpcCmdlet: YcBase<NetworkServiceClient>
    {
        [Parameter(Mandatory = true)]
        public string NetworkId;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;
        protected override void ProcessRecord()
        {
            var deleteNetworkRequest = new DeleteNetworkRequest {NetworkId = NetworkId};
            
            // we need to create another client here
            if (Force.IsPresent)
            {
                DeleteNestedSubnets();
            }
            
            var res = base.grpcClient.Delete(deleteNetworkRequest, base.headers).WaitForCompletion().Result;
            WriteObject(res);
        }

        private void DeleteNestedSubnets()
        {
            var getNetworkRequest = new GetNetworkRequest {NetworkId = NetworkId};
            var networkDetails = base.grpcClient.Get(getNetworkRequest, base.headers);


            var endpointId = YcConfig.Instance.Configuration[$"TypeToEndpointsMappings:SubnetServiceClient"];
            if (string.IsNullOrEmpty(endpointId))
            {
                var ex = new Exception($"cannot resolve API endpoint from the SubnetClient type");
                var err = new ErrorRecord(ex, "100", ErrorCategory.MetadataError, null);
                ThrowTerminatingError(err);
            }

            string endpoint = YcConfig.Instance.Configuration[$"Settings:{endpointId}"];
            GrpcChannel grpcSubnetChannel = GrpcChannel.ForAddress($"https://{endpoint}");
            var grpcSubnetClient = new SubnetService.SubnetServiceClient(grpcSubnetChannel);

            var subnetRequest = new ListSubnetsRequest {FolderId = networkDetails.FolderId};
            var subnetList = grpcSubnetClient.List(subnetRequest, base.headers).Subnets.Where(s => s.NetworkId == NetworkId);

            foreach (var s in subnetList)
            {
                var deleteSubnetRequest = new DeleteSubnetRequest {SubnetId = s.Id};
                var deleteSubnetResult = grpcSubnetClient.Delete(deleteSubnetRequest, base.headers).WaitForCompletion().Result;
            }
        }
    }
}
