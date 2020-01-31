namespace WebApplication1.Data.App
{
    public class TerminalConfig : Entity<long>
    {
        public long TerminalId { get; set; }
        public Terminal Terminal { get; set; }
        public long ConfigId { get; set; }
        public TerminalConfigTemplate TerminalConfigTemplate { get; set; }
        public bool IsActive { get; set; }
    }
}
