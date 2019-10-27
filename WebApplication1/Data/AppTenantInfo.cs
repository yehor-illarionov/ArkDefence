using Finbuckle.MultiTenant.Stores;

namespace WebApplication1.Data
{
    public class AppTenantInfo : IEFCoreStoreTenantInfo
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ConnectionString { get; set; }
    }
}