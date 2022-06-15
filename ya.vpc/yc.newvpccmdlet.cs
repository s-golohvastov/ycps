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
            //base.ProcessRecord();
        }

        private async Task<Operation> WaitForOperationCompletion(Operation o)
        {
            var operationRequest = new GetOperationRequest { OperationId = o.Id };

            while (!o.Done)
            {
                o = base.grpcOperationClient.Get( operationRequest, base.headers);
                System.Threading.Thread.Sleep(333);
            }

            return o;
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
            var x = WaitForOperationCompletion(res).Result;
            var z = x.Response.Unpack<Network>();

            WriteObject(z);
        }
    }
}
