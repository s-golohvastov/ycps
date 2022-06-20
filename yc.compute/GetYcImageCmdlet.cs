using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Compute.V1;
using static Yandex.Cloud.Compute.V1.ImageService;
using yc.basecmdlet;

namespace yc.compute
{
    [Cmdlet(VerbsCommon.Get, "YcVmImage", DefaultParameterSetName = "ByFolderId")]
    public class GetYcImageCmdlet : YcBase<ImageServiceClient>
    {
        [Parameter(Mandatory = false, ParameterSetName = "ByFolderId")]
        [Parameter(Mandatory = false, ParameterSetName = "LatestByFamily")]
        public string FolderId = "standard-images";

        [Parameter(Mandatory = true, ParameterSetName = "LatestByFamily")]
        public string Family;
        
        //TODO: add tests
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByFolderId":
                    ByFolderId();
                    break;
                case "LatestByFamily":
                    LatestByFamily();
                    break;
            }
        }

        private void ByFolderId()
        {
            var request = new ListImagesRequest { FolderId = FolderId};
            var ret = base.grpcClient.List(request, base.headers).GetToEnd(request, base.grpcClient, base.headers);
            WriteObject(ret.Images, true);
        }

        private void LatestByFamily()
        {
            var request = new GetImageLatestByFamilyRequest {Family = Family, FolderId = FolderId};
            var ret = base.grpcClient.GetLatestByFamily(request, base.headers);
            WriteObject(ret);
        }
    }
}
