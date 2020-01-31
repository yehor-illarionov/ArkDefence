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
}
