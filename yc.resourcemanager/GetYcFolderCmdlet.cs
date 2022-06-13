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
    [Cmdlet(VerbsCommon.Get, "YcFolder")]
    public class GetYcFolderCmdlet : YcBase<FolderServiceClient>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public string? FolderId;
        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(FolderId))
            {
                // list
                var listFolderRequest = new ListFoldersRequest();
                var res = base.grpcClient.List(listFolderRequest, base.headers);
                WriteObject(res.Folders, true);

            }
            else
            {
                // specific folder
                var getFolderRequest = new GetFolderRequest { FolderId = FolderId };
                var res = base.grpcClient.Get(getFolderRequest, base.headers);
                WriteObject(res);
            }

        }
    }
}
