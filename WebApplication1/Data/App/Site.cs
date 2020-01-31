using Core.Domain;
using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.App;

namespace WebApplication1.Data
{
    [MultiTenant]
    public class Site:Entity<long>
    {
        public Site(string alias)
        {
            Alias = alias ?? throw new ArgumentNullException(nameof(alias));
        }

        public string Alias { get; set; }

        public List<SbcController> Controllers { get; set; }
    }
}
