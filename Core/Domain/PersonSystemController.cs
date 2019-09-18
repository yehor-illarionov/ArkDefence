using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class PersonSystemController
    {
        [Key]
        public long Id { get; set; }
        public long PersonId { get; set; }
        public Person Person { get; set; }
        public string SystemControllerId { get; set; }
        public SystemController SystemController { get; set; }
    }
}
