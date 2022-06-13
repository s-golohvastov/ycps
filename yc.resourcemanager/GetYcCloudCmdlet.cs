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
    [Cmdlet(VerbsCommon.Get, "YcCloud", DefaultParameterSetName = "SingleCloud")]
    public class GetYcCloudCmdlet : YcBase<CloudServiceClient>
    {
        [Parameter(ParameterSetName = "SingleCloud")]
        public string? CloudId;

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "SingleCloud":
                    SingleCloud(grpcClient);
                    break;

            }
        }

        private void SingleCloud(CloudService.CloudServiceClient c)
        {
            if (string.IsNullOrEmpty(CloudId))
            {
                var request = new ListCloudsRequest();
                var cloudList = c.List(request, base.headers);
                WriteObject(cloudList.Clouds, true);
            }
            else
            {
                var request = new GetCloudRequest { CloudId = CloudId };
                var cloud = c.Get(request, base.headers);
                WriteObject(cloud);

            }
        }
    }
}