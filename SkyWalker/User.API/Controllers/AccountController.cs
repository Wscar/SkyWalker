using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.API.ViewModel;
using User.API.DBContext;
using User.API.Entities;
namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        public readonly UserDbContext UserDbContext;
        public AccountController(UserDbContext _userDbContext)
        {
            this.UserDbContext = _userDbContext;
        }
        [HttpPost]
        [Route("register")]
        public async Task< IActionResult> Register([FromForm] RegisterViewModel viewModel)
        {
            var appUser = new AppUser
            {
                UserId = viewModel.UserId,
                UserPassWord = viewModel.Password
            };
            await UserDbContext.AppUsers.AddAsync(appUser);
            await UserDbContext.SaveChangesAsync();
            return Json(new { Status = "success", Msg = "注册成功" });
        }
        
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpGet]
        [Route("test")]
        public  IActionResult Test()
        {
            return new JsonResult(new { Name = "张三" });
        }
            
    }
}