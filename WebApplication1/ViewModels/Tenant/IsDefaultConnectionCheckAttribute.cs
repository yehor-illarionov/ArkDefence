using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModels;

namespace WebApplication1.Attributes
{
    public class IsDefaultConnectionSaveTenantAttribute : ValidationAttribute
    {
        public IsDefaultConnectionSaveTenantAttribute()
        {
            ErrorMessage = "if isDefaultConnection set to false, connectionString cannot be empty";
        }
        public override bool IsValid(object value)
        {
            SaveTenant tenant =(SaveTenant)value;
            if (tenant.IsDefaultConnection == false
               && string.IsNullOrWhiteSpace(tenant.ConnectionString))
            {
                return false;
            }
            return true;
        }
    }
}
