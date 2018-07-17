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
using User.API.Dtos;
using User.API.Exceptions;
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
            UserDto userDto = new UserDto();           
            //var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == id);
                var user = await userRepository.GetAsync(id);
                if (user == null)
                {
                    userDto.Status = EntityStatus.Fail;
                    var ex = new SkyWalkerException($"错误的用户id:{id}");
                    throw ex;
                }
                else
                {
                    userDto.Status = EntityStatus.Suceess;
                    userDto.User = user;
                }                                
            return Json(userDto);

            // return Json(userDto);
        }
        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> UpdateUserAsync([FromBody]JsonPatchDocument<AppUser> patch)
        {
            UserDto userDto = new UserDto();
            var user = await userRepository.GetAsync(1);
            patch.ApplyTo(user);
            var dbResult = await userRepository.UpdateAsync(user);
           
            if (dbResult > 0)
            {
                userDto.Status = EntityStatus.Suceess;
                userDto.User = user;
            }
            else
            {
                userDto.Status = EntityStatus.Fail;
                userDto.Message = "更新失败";
            }          
            return Json(userDto);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            UserDto userDto = new UserDto();
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == id);

            var dbResult = await userRepository.DeltetAsync(user);
            if (dbResult > 0)
            {
                userDto.Status = EntityStatus.Suceess;
                userDto.User = user;
            }
            else
            {
                userDto.Status = EntityStatus.Fail;
                userDto.Message = "删除失败";
            }
           
            return Json(user);
        }
    }
}