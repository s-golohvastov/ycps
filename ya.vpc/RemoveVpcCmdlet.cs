using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Vpc.V1;
using yc.basecmdlet;
using static Yandex.Cloud.Vpc.V1.NetworkService;


namespace yc.vpc
{
    
    [Cmdlet(VerbsCommon.Remove, "YcVpc")]
    public class RemoveVpcCmdlet: YcBase<NetworkServiceClient>
    {
        [Parameter(Mandatory = true)]
        public string NetworkId;
        protected override void ProcessRecord()
        {
            var deleteNetworkRequest = new DeleteNetworkRequest {NetworkId = NetworkId};
            var res = base.grpcClient.Delete(deleteNetworkRequest, base.headers).WaitForCompletion().Result;
            WriteObject(res);
        }
    }
}
