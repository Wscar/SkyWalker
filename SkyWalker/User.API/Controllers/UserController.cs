using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyWalker.Dal.DBContext;
using SkyWalker.Dal.Entities;
namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly SkyWalkerDbContext dbContext;
        public UserController(SkyWalkerDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserAsync(int id) 
        {
            Dictionary<string, AppUser> result = new Dictionary<string, AppUser>();
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                result.Add("error", null);
            }
            else
            {
                result.Add("success", user);
            }
                
            return Json(result);
        }
        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> UpdateUserAsync([FromBody]JsonPatchDocument<AppUser> patch)
        {
            Dictionary<string, AppUser> result = new Dictionary<string, AppUser>();
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == 1);
            patch.ApplyTo(user);
            user= dbContext.AppUsers.Update(user).Entity;
            var dbResult= await dbContext.SaveChangesAsync();
            if (dbResult > 0)
            {
                result.Add("success", user);
            }
            else
            {
                result.Add("fail", user);
            }          
            return Json(result);
        }
        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == id);
             dbContext.AppUsers.Remove(user);
            var dbResult= await dbContext.SaveChangesAsync();
            if (dbResult > 0)
            {
              
                result.Add("del_user", true);
            }
            else
            {
                result.Add("del_user", false);
            }
           
            return Json(result);
        }
    }
}