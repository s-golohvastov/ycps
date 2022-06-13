using Grpc.Core;
using Grpc.Net.Client;
using System.Management.Automation;
using Yandex.Cloud.Organizationmanager.V1;
using Yandex.Cloud.Resourcemanager.V1;
using yc.auth;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Resourcemanager.V1.CloudService;

namespace yc.resourcemanager
{
    [Cmdlet(VerbsCommon.Get, "YcCloud")]
    public class GetYcCloudCmdlet : YcBase<CloudServiceClient>
    {
        [Parameter(ParameterSetName = "SingleCloud")]
        public string CloudId;

        [Parameter(ParameterSetName = "ListByOrgString")]
        public string OrganizationId;

        [Parameter(ParameterSetName = "ListByOrgObj")]
        public Organization Organization;

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "ListByOrgObj":
                    ListByOrgObj(grpcClient, headers);
                    break;
                case "ListByOrgString":
                    ListByOrgString(grpcClient, headers);  //TODO: cccc
                    break;
                case "SingleCloud":
                    SingleCloud(grpcClient, headers);
                    break;

            }
        }

        private void ListByOrgObj(CloudService.CloudServiceClient c, Metadata h)
        {
            var request = new ListCloudsRequest { };
            var cloud = c.List(request, h);
            WriteObject(cloud, true);
        }

        private void ListByOrgString(CloudService.CloudServiceClient c, Metadata h)
        {
            var request = new ListCloudsRequest { OrganizationId = OrganizationId };
            var cloud = c.List(request, h);
            WriteObject(cloud, true);
        }

        private void SingleCloud(CloudService.CloudServiceClient c, Metadata h)
        {
            var request = new GetCloudRequest { CloudId = CloudId };
            var cloud = c.Get(request, h);
            WriteObject(cloud, true);
        }
    }
}