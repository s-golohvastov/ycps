using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Resourcemanager.V1;
using Yandex.Cloud.Vpc.V1;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Vpc.V1.SubnetService;

namespace yc.vpc
{
    [Cmdlet(VerbsCommon.Get, "YcSubnet", DefaultParameterSetName = "FolderId")]
    public class YcGetSubnetCmdlet: YcBase<SubnetServiceClient>
    {
        [Parameter(Mandatory = true, ParameterSetName = "FolderId")]
        [Parameter(Mandatory = true, ParameterSetName = "FolderIdNameFiler")]
        [ValidateNotNullOrEmpty]
        public string? FolderId;
        
        [Parameter(Mandatory = true, ParameterSetName = "FolderObj", ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "FolderObjNameFiler", ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public Folder? FolderObj;

        [Parameter(Mandatory = false, ParameterSetName = "FolderIdNameFiler")]
        [Parameter(Mandatory = false, ParameterSetName = "FolderObjNameFiler")]
        [Alias("Name")]
        public string? SubnetName;


        [Parameter(Mandatory = true, ParameterSetName = "SubnetId")]
        [ValidateNotNullOrEmpty]
        [Alias("Id")]
        public string? SubnetId;


        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "FolderId":
                    GetAllSubnetsInFolder(FolderId);
                    break;
                case "FolderObj":
                    GetAllSubnetsInFolder(FolderObj.Id);
                    break;
                case "FolderIdNameFiler":
                    FilterSubnetsInFolderByName(FolderId, SubnetName);
                    break;
                case "FolderObjNameFiler":
                    FilterSubnetsInFolderByName(FolderObj.Id, SubnetName);
                    break;
                case "SubnetId":
                    GetSubnetById(SubnetId);
                    break;
            }
        }

        private void GetAllSubnetsInFolder(string folderId)
        {
            var listRequest = new ListSubnetsRequest
            {
                PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]), 
                FolderId = folderId
            };
            var res = base.grpcClient.List(listRequest, base.headers);
            WriteObject(res.Subnets, true);
        }

        private void GetSubnetById(string subnetId)
        {
            var getRequest = new GetSubnetRequest() { SubnetId = subnetId };
            var res = base.grpcClient.Get(getRequest, base.headers);
            WriteObject(res);
        }

        private void FilterSubnetsInFolderByName(string folderId, string subnetName)
        {
            var listRequest = new ListSubnetsRequest
            {
                PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]), 
                Filter = $"name = \"{ subnetName }\"",
                FolderId = folderId
            };
            var res = base.grpcClient.List(listRequest, base.headers);
            WriteObject(res.Subnets, true);
        }
    }
}
