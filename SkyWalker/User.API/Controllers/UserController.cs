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
using SkyWalker.Dal.Repository;
namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly SkyWalkerDbContext dbContext;
        private readonly IRepository<AppUser> userRepository;
        public UserController(SkyWalkerDbContext _dbContext, IRepository<AppUser> _repository)
        {
            dbContext = _dbContext;
            userRepository = _repository;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserAsync(int id) 
        {
            Dictionary<string, AppUser> result = new Dictionary<string, AppUser>();
            //var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == id);
            var user = await userRepository.GetAsync(id);
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
            var user = await userRepository.GetAsync(1);
            patch.ApplyTo(user);
            var dbResult = await userRepository.UpdateAsync(user);
           
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
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == id);

            var dbResult = await userRepository.DeltetAsync(user);
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