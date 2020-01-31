using Core.Domain;
using System;
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
}
