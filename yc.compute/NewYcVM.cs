using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Yandex.Cloud.Compute.V1;
using yc.basecmdlet;
using static Yandex.Cloud.Compute.V1.InstanceService;

namespace yc.compute
{
    [Cmdlet(VerbsCommon.New, "YcVM")]
    public class NewYcVM : YcBase<InstanceServiceClient>
    {
        [Parameter(Mandatory = true)]
        public string Name;

        [Parameter(Mandatory = true)]
        public string FolderId;

        [Parameter(Mandatory = true)]
        public string ZoneId;

        [Parameter(Mandatory = true)]
        public string PlatformId;

        [Parameter(Mandatory = true)]
        public string SubnetId;

        [Parameter(Mandatory = true)]
        public ResourcesSpec ResourceSpec;

        [Parameter(Mandatory = true)]
        public AttachedDiskSpec BootDiskSpec;

        [Parameter(Mandatory = false)]
        public AttachedDiskSpec? DataDiskSpec = null;

        [Parameter(Mandatory = false)]
        public System.Collections.Hashtable Metadata;


        protected override void ProcessRecord()
        {

            var nic = new NetworkInterfaceSpec
            {
                SubnetId = SubnetId,
                PrimaryV4AddressSpec = new PrimaryAddressSpec()
            };

            var request = new CreateInstanceRequest
            {
                Name = Name,
                FolderId = FolderId,
                ZoneId = ZoneId,
                PlatformId = PlatformId,
                NetworkInterfaceSpecs = { nic },
                BootDiskSpec = BootDiskSpec,
                ResourcesSpec = ResourceSpec
            };

            if (Metadata != null)
            {
                foreach (var key in Metadata.Keys)
                {
                    request.Metadata.Add(key.ToString(), Metadata[key].ToString());
                }
                
            }

            if (DataDiskSpec != null)
            {
                request.SecondaryDiskSpecs.Add(DataDiskSpec);
            }

            var ret = base.grpcClient.Create(request, base.headers).WaitForCompletion().Result;
            var z = ret.Response.Unpack<Instance>();

            WriteObject(z);
        }
    }
}
