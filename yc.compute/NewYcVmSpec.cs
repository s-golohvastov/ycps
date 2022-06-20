using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Compute.V1;
using yc.basecmdlet;
using static Yandex.Cloud.Compute.V1.InstanceService;

namespace yc.compute
{
    [Cmdlet(VerbsCommon.New, "YcVmSpecification")]
    [Alias("New-YcVmSpec")]
    public class NewYcVmSpecification: YcBaseCmdlet
    {
        [Parameter(Mandatory = true)]
        public long Memory;
        
        [Parameter(Mandatory = true)]
        public int Cores;
        
        [Parameter(Mandatory = false)]
        public int CoreFraction = 50;

        [Parameter(Mandatory = false)]
        public int Gpus = 0;
        protected override void ProcessRecord()
        {
            WriteObject( new ResourcesSpec { Memory = Memory, Cores = Cores, CoreFraction = CoreFraction, Gpus = Gpus}  );
        }
    }
}
