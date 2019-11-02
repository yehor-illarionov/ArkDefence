using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.App
{
    public class Hardware : Entity<long>
    {
        public string Alias { get; set; }
        public string VendorId { get; set; }
        public HardwareTypes Type { get; set; }
        public string Comment { get; set; }

        public HardwareVersionIndex VersionIndex { get; set; }
    }

    public enum HardwareTypes
    {
        NotSet=0,
        MicroController,
        Finger,
        CardReader,
        Camera,
        Sbc,
        MotherBoard
    }

    public class HardwareVersionIndex : Entity<long>
    {
        public string Alias { get; set; }
        public string Comment { get; set; }

        public List<Hardware> Hardwares { get; set; }
    }

    public class ControllerVersionIndex: HardwareVersionIndex
    {
        public List<SbcController> Controllers { get; set; }
    }

    public class TerminalVersionIndex : HardwareVersionIndex
    {
        public List<Terminal> Terminals { get; set; }
        public List<TerminalConfigTemplate> ConfigTemplates { get; set; }
    }
}
