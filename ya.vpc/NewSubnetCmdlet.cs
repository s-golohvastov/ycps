using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Vpc.V1;
using yc.basecmdlet;
using static Yandex.Cloud.Vpc.V1.SubnetService;

namespace yc.vpc
{
    [Cmdlet(VerbsCommon.New, "YcSubnet")]
    public class YcNewSubnetCmdlet : YcBase<SubnetServiceClient>
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string FolderId;
        
        [Parameter(Mandatory = true, Position = 1)]
        public string NetworkId;
        
        [Parameter(Mandatory = true, Position = 2)]
        public string Name;

        [Parameter(Mandatory = true, Position = 3)]
        public string ZoneId;

        [Parameter(Mandatory = true)]
        public string[] Ipv4Ranges;

        protected override void ProcessRecord()
        {
            var createSubnetRequest = new CreateSubnetRequest
            {
                Name = Name,
                NetworkId = NetworkId,
                FolderId = FolderId,
                ZoneId = ZoneId,
                V4CidrBlocks = { Ipv4Ranges }
            };
            var res = base.grpcClient.Create( createSubnetRequest , base.headers).WaitForCompletion().Result;
            WriteObject(res);
        }
    }
}
