using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Compute.V1;
using yc.basecmdlet;
using static Yandex.Cloud.Compute.V1.InstanceService;

namespace yc.compute
{
    [Cmdlet(VerbsCommon.Remove, "YcVM")]
    public class RemoveYcVmCmdlet : YcBase<InstanceServiceClient>
    {
        [Parameter(Mandatory = true)] public string VmId;

        protected override void ProcessRecord()
        {
            var request = new DeleteInstanceRequest {InstanceId = VmId};
            var ret = base.grpcClient.Delete(request, base.headers).WaitForCompletion().Result;
            WriteObject(ret);
        }
    }
}
