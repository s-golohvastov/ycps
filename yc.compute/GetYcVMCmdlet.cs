using System.Management.Automation;
using Yandex.Cloud.Compute.V1;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Compute.V1.InstanceService;

namespace yc.compute
{
    [Cmdlet(VerbsCommon.Get, "YcCmdlet", DefaultParameterSetName = "ByInstanceId")]
    public class GetYcVMCmdlet : YcBase<InstanceServiceClient>
    {
        [Parameter(ParameterSetName = "ByInstanceId")]
        public string? InstanceId;

        [Parameter(ParameterSetName = "ByInstanceName")]
        public string? InstanceName;

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "ByInstanceId":
                    GetVmById();
                    break;
                case "ByInstanceName":
                    GetVmByName();
                    break;
                default:
                    break;
            }
        }

        private void GetVmByName()
        {
            var res = base.grpcClient.List(new ListInstancesRequest { Filter = $"[Instance.name] = {InstanceName}" }, base.headers);
            WriteObject(res.Instances, true);
        }

        private void GetVmById()
        {
            if (string.IsNullOrEmpty(InstanceId))
            {
                // list
                var res = base.grpcClient.List(new ListInstancesRequest { PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]) }, base.headers);
                WriteObject(res.Instances, true);
            }
            else
            {
                // get by ID
                var res = base.grpcClient.Get(new GetInstanceRequest { InstanceId = InstanceId }, base.headers);
                WriteObject(res);
            }
        }
    }
}