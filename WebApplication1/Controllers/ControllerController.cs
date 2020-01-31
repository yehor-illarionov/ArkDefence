using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Exceptions;

namespace WebApplication1.Controllers
{
    [Route("{__tenant__=}/api/[controller]")]
    [ApiController]
    public class ControllerController : ControllerBase
    {
        private readonly IDataProtectionProvider provider;

        public ControllerController(IDataProtectionProvider provider)
        {
            this.provider = provider;
        }

        // GET: api/Controller
        [HttpGet]
        public async Task<IEnumerable<ResController>> Get()
        {
            var tenantInfo = HttpContext.GetMultiTenantContext().TenantInfo;
            var repo = new SystemControllerRepository(tenantInfo, provider);
            return await repo.GetAll();
        }

        // GET: api/Controller/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Controller
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateControllerDto value)
        {
            var tenantInfo = HttpContext.GetMultiTenantContext().TenantInfo;
            var repo = new SystemControllerRepository(tenantInfo, provider);
            bool res=await repo.Add(value);
            if (!res)
            {
                throw new HttpResponseException() { Status=403};
            }return Ok();
        }

        // PUT: api/Controller/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
