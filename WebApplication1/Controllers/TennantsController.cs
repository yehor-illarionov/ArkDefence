using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TennantsController:ControllerBase
    {
        private readonly TennantRepository repository;

        public TennantsController(TennantRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var temp = new Core.Domain.Tennant();
            temp.Alias = "webtest";
            await repository.AddAsync(temp);
            return Ok();
        }

    }
}
