using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Yandex.Cloud.Operation;
using Yandex.Cloud.Vpc.V1;
using yc.basecmdlet;
using yc.config;
using static Yandex.Cloud.Vpc.V1.NetworkService;

namespace yc.vpc
{
 
    [Cmdlet(VerbsCommon.New, "YcVpc")]
    public class YcNewVpcCmdlet: YcBase<NetworkServiceClient>
    {

        [Parameter(Mandatory = true)]
        public string FolderId;
        
        [Parameter(Mandatory = true)]
        public string Name;
        
        [Parameter(Mandatory = true)] 
        public string Description;

        protected override void ProcessRecord()
        {
            CreateVpc(FolderId, Name, Description);
        }

        private void CreateVpc(string folderId, string name, string description, MapField<string, string>? labels = default)
        {
            CreateNetworkRequest createRequest;
            if (null == labels)
            {
                createRequest = new CreateNetworkRequest
                {
                    FolderId = folderId,
                    Description = description,
                    Name = name
                };
            }
            else
            {
                createRequest = new CreateNetworkRequest
                {
                    FolderId = folderId,
                    Description = description,
                    Name = name,
                    Labels = { labels }
                };
            }

            Operation res = base.grpcClient.Create(createRequest, base.headers);
            var operationResult = res.WaitForCompletion().Result;
            
            var z = operationResult.Response.Unpack<Network>();
            
            WriteObject(z);
        }
    }
}
