using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Data.App;

namespace Core.Domain
{
    /// <summary>
    /// Application Hash Context : App.v1
    /// </summary>
    public class SbcController : Entity<long>
    {
        public SbcController() { }
        public SbcController(string alias)
        {
            Alias = alias;
        }
        public string Alias { get; set; }
        public string ClientIdHash { get; set; }
        public string ClientSecretHash { get; set; }
        public string Mac { get; set; }

        public Site Site { get; set; }
        public ControllerVersionIndex VersionIndex { get; set; }
        public List<Terminal> Terminals { get; set; }
    }
}
