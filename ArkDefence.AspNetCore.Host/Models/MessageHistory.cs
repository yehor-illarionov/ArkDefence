using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArkDefence.AspNetCore.Host.Models
{
    public class MessageHistory
    {
        public MessageHistory(string method, string data)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Data = data ?? throw new ArgumentNullException(nameof(data));

            Timestamp = DateTime.UtcNow;
        }

        [Key]
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Method { get; set; }
        public string Data { get; set; }
    }
}
