using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkDefence.AspNetCore.Host.Models
{
    public class Terminal : Entity<string>
    {
        public Terminal(string id):base(id)
        {
            //Id = id ?? throw new ArgumentNullException(nameof(id));
            this.IsDatabaseFull = false;
        }

        public string Alias { get; set; }
        public string Port { get; set; }
        public bool IsDatabaseFull { get; set; }

        public string JsonConfig { get; set; }
        public string JsonConfigSchema { get; set; }
        public int JsonConfigVersion { get; set; }


        #region terminal_configuration
        public int FingerTimeout { get; set; }
        public bool IsFingerEnabled { get; set; }
        public bool IsBleEnabled { get; set; }
        public bool IsCardEnabled { get; set; }
        public bool IsStrictMode { get; set; }
        #endregion

        public string SystemControllerId { get; set; }
        public SystemController SystemController { get; set; }
    }
}
