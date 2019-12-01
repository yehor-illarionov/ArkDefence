using System;

namespace WebApplication1.Constants
{
    public static class TenantsControllerRoute
    {
        public const string DeleteTenant = ControllerName.Tenant + nameof(DeleteTenant);
        public const string GetTenant = ControllerName.Tenant + nameof(GetTenant);
        public const string GetTenantPage = ControllerName.Tenant + nameof(GetTenantPage);
        public const string HeadTenant = ControllerName.Tenant + nameof(HeadTenant);
        public const string HeadTenantPage = ControllerName.Tenant + nameof(HeadTenantPage);
        public const string PatchTenant = ControllerName.Tenant + nameof(PatchTenant);
        public const string PostTenant = ControllerName.Tenant + nameof(PostTenant);
        public const string PutTenant = ControllerName.Tenant + nameof(PutTenant);
    }
}
