using System.Management.Automation;
using Yandex.Cloud.Compute.V1;
using Yandex.Cloud.Resourcemanager.V1;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Compute.V1.InstanceService;

namespace yc.compute
{
    [Cmdlet(VerbsCommon.Get, "YcVM", DefaultParameterSetName = "AllInFolderId")]
    public class GetYcVMCmdlet : YcBase<InstanceServiceClient>
    {
        [Parameter(ParameterSetName = "AllInFolderId", Mandatory = true)]
        [Parameter(ParameterSetName = "FilterByInstanceName", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string? FolderId;

        [Parameter(ParameterSetName = "AllInFolderObj", Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = "FilterFolderObjByInstanceName", Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public Folder? Folder;

        [Parameter(ParameterSetName = "ByInstanceId")]
        public string? InstanceId;

        [Parameter(ParameterSetName = "FilterByInstanceName", Mandatory = true)]
        [Parameter(ParameterSetName = "FilterFolderObjByInstanceName", Mandatory = true)]
        public string? InstanceName;

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "ByInstanceId":
                    GetVmById();
                    break;
                case "FilterByInstanceName":
                    GetVmByName(FolderId, InstanceName);
                    break;
                case "AllInFolderId":
                    GetVmByFolderId(FolderId);
                    break;
                case "AllInFolderObj":
                    GetVmByFolderObj();
                    break;
                case "FilterFolderObjByInstanceName":
                    FilterFolderObjByInstanceName();
                    break;
                default:
                    break;
            }
        }

        private void FilterFolderObjByInstanceName()
        {
            GetVmByName(Folder.Id, InstanceName);
        }

        private void GetVmByFolderObj()
        {
            GetVmByFolderId(Folder.Id);
        }

        private void GetVmByFolderId(string id)
        {
            var res = base.grpcClient.List(new ListInstancesRequest { FolderId = id, PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]) }, base.headers);
            WriteObject(res.Instances, true);
        }

        private void GetVmByName(string id, string name)
        {
            var res = base.grpcClient.List(new ListInstancesRequest { Filter = $"name=\"{name}\"", FolderId = id }, base.headers);
            WriteObject(res.Instances, true);
        }

        private void GetVmById()
        {
            // get by ID
            var res = base.grpcClient.Get(new GetInstanceRequest { InstanceId = InstanceId }, base.headers);
            WriteObject(res);
        }
    }
}