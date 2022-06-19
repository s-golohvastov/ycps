using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Compute.V1;
using static Yandex.Cloud.Compute.V1.ZoneService;
using yc.basecmdlet;

namespace yc.compute
{
    [Cmdlet(VerbsCommon.Get, "YcZone")]
    public class GetYcZone : YcBase<ZoneServiceClient>
    {

        // TODO: create pester tests
        protected override void ProcessRecord()
        {
            var zoneRequest = new ListZonesRequest();
            var res = base.grpcClient.List(zoneRequest, base.headers);
            WriteObject(res.Zones, true);
        }
    }
}
