using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Resourcemanager.V1;
using yc.basecmdlet;
using static Yandex.Cloud.Resourcemanager.V1.FolderService;

namespace yc.resmgr
{
    [Cmdlet(VerbsCommon.Get, "YcFolder", DefaultParameterSetName = "CloudId")]
    public class GetYcFolderCmdlet : YcBase<FolderServiceClient>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = "FolderId")]
        [ValidateNotNullOrEmpty]
        public string? FolderId;

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "CloudId")]
        [ValidateNotNullOrEmpty]
        public string? CloudId;

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Cloud")]
        [ValidateNotNullOrEmpty]
        public Cloud[] Cloud;
        protected override void ProcessRecord()
        {

            switch (this.ParameterSetName)
            {
                case "CloudId":
                    ListFoldersByCloudId(CloudId);
                    break;
                case "FolderId":
                    GetFolderByFolderId();
                    break;
                case "Cloud":
                    GetFolderByCloudObj();
                    break;
            }

        }

        private void GetFolderByCloudObj()
        {
            foreach (var c in Cloud)
            {
                ListFoldersByCloudId(c.Id);
            }
        }

        private void GetFolderByFolderId()
        {
            // specific folder
            var getFolderRequest = new GetFolderRequest { FolderId = FolderId };
            var res = base.grpcClient.Get(getFolderRequest, base.headers);
            WriteObject(res);
        }

        private void ListFoldersByCloudId(string id)
        {
            // list
            var listFoldersRequest = new ListFoldersRequest { CloudId = id} ;
            var res = base.grpcClient.List(listFoldersRequest, base.headers);
            WriteObject(res.Folders, true);
        }
    }
}
