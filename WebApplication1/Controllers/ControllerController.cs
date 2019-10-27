using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("{__tenant__=}/api/[controller]")]
    [ApiController]
    public class ControllerController : ControllerBase
    {
        private NextAppContext context;

        public ControllerController(NextAppContext context)
        {
            this.context = context;
        }

        // GET: api/Controller
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //context.Add(new SbcController("some1"));
            var temp = context.Controllers.Find((long)1);
            temp.SoftDelete();
            context.Update(temp);
            context.SaveChanges();
            return new string[] { "value1", "value2" };
        }

        // GET: api/Controller/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Controller
        [HttpPost]
        public void Post([FromBody] string value)
        {

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
