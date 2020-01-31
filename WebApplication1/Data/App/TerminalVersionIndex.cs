using System.Collections.Generic;

namespace WebApplication1.Data.App
{
    public class TerminalVersionIndex : HardwareVersionIndex
    {
        public List<Terminal> Terminals { get; set; }
        public List<TerminalConfigTemplate> ConfigTemplates { get; set; }
    }
}
