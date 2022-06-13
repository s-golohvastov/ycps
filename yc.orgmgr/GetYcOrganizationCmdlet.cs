using Grpc.Core;
using Grpc.Net.Client;
using System.Management.Automation;
using Yandex.Cloud.Organizationmanager.V1;
using Yandex.Cloud.Resourcemanager.V1;
using yc.auth;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Organizationmanager.V1.OrganizationService;

namespace yc.orgmgr
{
    [Cmdlet(VerbsCommon.Get, "YcOrganization")]
    public class GetYcOrganizationCmdlet : YcBase<OrganizationServiceClient>
    {
    
        protected override void ProcessRecord()
        {
            var req = new ListOrganizationsRequest {  };
            req.PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]);
            var ret = base.grpcClient.List(req, headers);
            WriteObject(ret.Organizations, true);
        }
    }
}