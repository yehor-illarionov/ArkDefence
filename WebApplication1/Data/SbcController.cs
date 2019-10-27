using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    [MultiTenant]
    public class SbcController : Entity<long>
    {
        public SbcController(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; set; }
    }
}
