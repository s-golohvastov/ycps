using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Yandex.Cloud.Compute.V1;
using yc.config;

namespace yc.basecmdlet
{
    // TODO: add pester tests
    [Cmdlet(VerbsCommon.New, "YcAttachedDiskSpecification")]
    [Alias("New-YcAttachedDiskSpec")]
    public class NewYcAttachedDiskSpecification : YcBaseCmdlet
    {

        [Parameter(Mandatory = true)]
        public string Name;

        [Parameter(Mandatory = false)]
        public AttachedDiskSpec.Types.Mode Mode = AttachedDiskSpec.Types.Mode.ReadWrite;

        [Parameter(Mandatory = true)]
        public AttachedDiskSpec.Types.DiskSpec DiskSpec;

        [Parameter(Mandatory = false)]
        public bool AutoDelete = true;


        protected override void ProcessRecord()
        {

            var spec = new AttachedDiskSpec
            {
                DiskSpec = DiskSpec,
                DeviceName = Name,
                Mode = Mode,
                AutoDelete = AutoDelete
            };

            spec.Mode = AttachedDiskSpec.Types.Mode.ReadOnly;

            WriteObject(spec);
            //base.ProcessRecord();
        }
    }

    // TODO: add image or snapshot support
    // TODO: add pester tests
    // TODO: add placement policy
    [Cmdlet(VerbsCommon.New, "YcDiskSpecification")]
    [Alias("New-YcDiskSpec")]
    public class NewYcDiskSpecification : YcBaseCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name;

        [Parameter(Mandatory = false)]
        [StringLength(256)]
        public string Description = String.Empty;

        [Parameter(Mandatory = false)]
        public string TypeId;

        [Parameter(Mandatory = true)]
        public long Size;

        [Parameter(Mandatory = false)]
        public int Blocksize = 8192;

        [Parameter(Mandatory = false)] 
        public bool Autodelete = true;

        [Parameter(Mandatory = true)] 
        public string ImageId;

        protected override void ProcessRecord()
        {

            var spec = new AttachedDiskSpec.Types.DiskSpec
            {
                Name = Name,
                Description = Description,
                TypeId = TypeId,
                Size = Size,
                BlockSize = Blocksize,
                ImageId = ImageId
            };

            var attachedDevice = new AttachedDiskSpec
            {
                AutoDelete = Autodelete,
                DiskSpec = spec

            };

            WriteObject(attachedDevice);
        }
    }

    // TODO: add pester tests
    [Cmdlet(VerbsCommon.Get, "YcDiskType")]
    public class YcGetDiskType : YcBase<DiskTypeService.DiskTypeServiceClient>
    {
        protected override void ProcessRecord()
        {
            var request = new ListDiskTypesRequest { PageSize = int.Parse(YcConfig.Instance.Configuration["Settings:defaultPageSize"]) };
            var ret = base.grpcClient.List(request, base.headers);
            WriteObject(ret.DiskTypes);
        }
    }
}