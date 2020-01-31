using System.Collections.Generic;

namespace WebApplication1.Data.App
{
    public class HardwareVersionIndex : Entity<long>
    {
        public string Alias { get; set; }
        public string Comment { get; set; }

        public List<Hardware> Hardwares { get; set; }
    }
}
