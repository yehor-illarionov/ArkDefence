using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.App;

namespace WebApplication1.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class TestController : ControllerBase
    //{
    //    private readonly IMultiTenantStore context;
    //    private readonly SystemControllerRepository crepo;
    //    private readonly TerminalRepository trepo;

    //    public TestController(IMultiTenantStore context, SystemControllerRepository crepo, TerminalRepository trepo)
    //    {
    //        this.context = context;
    //        this.crepo = crepo;
    //        this.trepo = trepo;
    //    }


    //    // GET: api/Test
    //    [HttpGet]
    //    public async Task<IEnumerable<ResController>> Get()
    //    {
    //        //var temp = new List<TenantInfo>();
    //        //temp.Add(new TenantInfo("tenant-egorliberty", "egor", "Egor", "Server=127.0.0.1;Port=5432;Database=arkdefence_eg;User Id=postgres;Password=BmHkYi5436!;", null));
    //        //temp.Add(new TenantInfo("tenant-vladimer-d043favoiaw", "vladimer", "Vladimer", "Server=127.0.0.1;Port=5432;Database=arkdefence_vv;User Id=postgres;Password=BmHkYi5436!;", null));
    //        //foreach(var item in temp)
    //        //{
    //        //    context.TryAddAsync(item).Wait();
    //        //    yield return new string("tenant added");
    //        //}

    //        //var sbc = new ResController("test", "testid", "testsecret");
    //        //  repo.AddController(sbc);
    //      //  var terminal = new Terminal() { Alias="some_terminal", Deleted=false, GpioPort=1, Port="/dev/ttyS1"};
    //       // trepo.Add(terminal);
    //        await crepo.AddTerminalAsync(1, 1);
    //        return await crepo.GetAllAsync();
           
    //    }



    //    // POST: api/Test
    //    [HttpPost]
    //    public void Post([FromBody] string value)
    //    {
    //    }

    //    // PUT: api/Test/5
    //    [HttpPut("{id}")]
    //    public void Put(int id, [FromBody] string value)
    //    {
    //    }

    //    // DELETE: api/ApiWithActions/5
    //    [HttpDelete("{id}")]
    //    public void Delete(int id)
    //    {
    //    }
    //}
}
