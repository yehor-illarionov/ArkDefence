using System.Collections.Generic;

namespace WebApplication1.Data.App
{
    public class TerminalConfigTemplate : Entity<long>
    {
        public string Alias { get; set; }
        public int FingerTimeout { get; set; }//in seconds
        public int BleDistance { get; set; }
        public bool IsBleEnabled { get; set; }
        public bool IsCardEnabled { get; set; }
        public bool IsFingerEnabled { get; set; }
        public bool HasBle { get; set; }
        public bool HasCard { get; set; }
        public bool HasFinger { get; set; }
        public TerminalModes Mode { get; set; }
        public TerminalVersionIndex HardwareIndex { get; set; }
        public ICollection<TerminalConfig> TerminalConfigs { get; set; }

    }
}
