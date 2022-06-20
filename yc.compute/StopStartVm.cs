using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Compute.V1;
using yc.basecmdlet;

namespace yc.compute
{
    [Cmdlet(VerbsLifecycle.Stop, "YcVm")]
    public class StopYcVmCmdlet : YcBase<InstanceService.InstanceServiceClient>
    {
        [Parameter(Mandatory = true)]
        public string InstanceId;

        protected override void BeginProcessing()
        {
            var request = new StopInstanceRequest {InstanceId = InstanceId};
            var res = base.grpcClient.Stop(request, base.headers).WaitForCompletion().Result;
            WriteObject(res);
        }
    }


    [Cmdlet(VerbsLifecycle.Start, "YcVm")]
    public class StartYcVmCmdlet : YcBase<InstanceService.InstanceServiceClient>
    {
        [Parameter(Mandatory = true)]
        public string InstanceId;

        protected override void BeginProcessing()
        {
            var request = new StartInstanceRequest {InstanceId = InstanceId};
            var res = base.grpcClient.Start(request, base.headers).WaitForCompletion().Result;
            WriteObject(res);
        }
    }
}
