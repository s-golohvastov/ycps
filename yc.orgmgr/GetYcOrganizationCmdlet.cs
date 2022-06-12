using Grpc.Core;
using Grpc.Net.Client;
using System.Management.Automation;
using Yandex.Cloud.Organizationmanager.V1;
using Yandex.Cloud.Resourcemanager.V1;
using yc.auth;
using yc.basecmdlet;
using yc.config;

namespace yc.orgmgr
{
    [Cmdlet(VerbsCommon.Get, "YcOrganization")]
    public class GetYcOrganizationCmdlet : YcBaseCmdlet
    {

        
        private static string endpoint = YcConfig.GetEndpointById("organization-manager").Result;
        private static GrpcChannel grpcChannel = GrpcChannel.ForAddress($"https://{endpoint}");
        private static OrganizationService.OrganizationServiceClient grpcClient = new OrganizationService.OrganizationServiceClient(grpcChannel);

        private Metadata headers; 

        public GetYcOrganizationCmdlet()
        {
            headers = new Metadata();
            headers.Add("Authorization", $"Bearer {AuthCache.Instance.GetAuthHeader()}");
        }
        protected override void ProcessRecord()
        {
            var req = new ListOrganizationsRequest {  };
            req.PageSize = int.Parse(YcConfig.Configuration["Settings:defaultPageSize"]);
            var ret = grpcClient.List(req, headers);
            WriteObject(ret.Organizations, true);
        }
    }
}