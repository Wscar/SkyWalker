using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyWalker.Dal.DBContext;
namespace User.API.Controllers
{   
   
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public readonly SkyWalkerDbContext dbContext;
        public ValuesController(SkyWalkerDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var book= dbContext.Books.SingleOrDefault(x => x.Id == 1);
            return new List<string>() { book.BookName };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
