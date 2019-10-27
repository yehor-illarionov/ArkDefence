using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class InfoController: Controller
    {
        private readonly IMultiTenantStore context;

        public InfoController(IMultiTenantStore context)
        {
            this.context = context;
        }

        public  IActionResult Index()
        {
            var ti = HttpContext.GetMultiTenantContext()?.TenantInfo;
            return View(ti);
        }

    }
}
