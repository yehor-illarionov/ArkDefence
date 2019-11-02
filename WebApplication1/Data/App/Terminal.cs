using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.App
{
    public class Terminal:Entity<long>
    {
        public string Alias { get; set; }
        public string Uuid { get; set; }
        public string Port { get; set; }
        public int GpioPort { get; set; }

        public SbcController Controller { get; set; }
        public TerminalVersionIndex VersionIndex { get; set; }
        public ICollection<TerminalConfig> TerminalConfigs { get; set; }
    }

    public class TerminalConfig : Entity<long>
    {
        public long TerminalId { get; set; }
        public Terminal Terminal { get; set; }
        public long ConfigId { get; set; }
        public TerminalConfigTemplate TerminalConfigTemplate { get; set; }
        public bool IsActive { get; set; }
    }

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

    public enum TerminalModes
    {
        NotSet=0,
        Strict,
        NotStrict
    }
}
