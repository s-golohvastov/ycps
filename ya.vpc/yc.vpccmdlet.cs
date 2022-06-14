using System.Management.Automation;
using System.Security.Policy;
using Yandex.Cloud.Resourcemanager.V1;
using Yandex.Cloud.Vpc.V1;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Vpc.V1.NetworkService;

namespace ya.vpc
{
    [Cmdlet(VerbsCommon.Get, "YcVpc", DefaultParameterSetName = "FolderId")]
    public class GetYcVpcCmdlet : YcBase<NetworkServiceClient>
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
        public string? NetworkName;


        [Parameter(Mandatory = true, ParameterSetName = "NetworkId")]
        [ValidateNotNullOrEmpty]
        public string? NetworkId;


        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "FolderId":
                    GetAllVpcInFolder(FolderId);
                    break;
                case "FolderObj":
                    GetAllVpcInFolder(FolderObj.Id);
                    break;
                case "FolderIdNameFiler":
                    FilterVpcInFolderByName(FolderId, NetworkName);
                    break;
                case "FolderObjNameFiler":
                    FilterVpcInFolderByName(FolderObj.Id, NetworkName);
                    break;
                case "NetworkId":
                    GetVpcById(NetworkId);
                    break;
            }
            base.ProcessRecord();
        }


        private void GetAllVpcInFolder(string folderId)
        {
            var listRequest = new ListNetworksRequest
            {
                PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]), 
                FolderId = folderId
            };
            var res = base.grpcClient.List(listRequest, base.headers);
            WriteObject(res.Networks, true);
        }

        private void GetVpcById(string networkId)
        {
            var getRequest = new GetNetworkRequest { NetworkId = networkId };
            var res = base.grpcClient.Get(getRequest, base.headers);
            WriteObject(res);
        }

        private void FilterVpcInFolderByName(string folderId, string networkName)
        {
            var listRequest = new ListNetworksRequest
            {
                PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]), 
                Filter = $"name = \"{ networkName }\"",
                FolderId = folderId
            };
            var res = base.grpcClient.List(listRequest, base.headers);
            WriteObject(res.Networks, true);
        }
    }
}