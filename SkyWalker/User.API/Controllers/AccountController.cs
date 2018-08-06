using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.API.ViewModel;
using SkyWalker.Dal.DBContext;
using SkyWalker.Dal.Entities;
using SkyWalker.Dal.Service;

namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }
        [HttpPost]
        [Route("register")]
        public async Task< IActionResult> Register([FromBody] RegisterViewModel viewModel)
        {
           
            return Json(new { Status = "success", Msg = "注册成功" });
        }
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult>SignIn(string userId,string passWord)
        {
            var result=await  accountService.SignInAsync(userId, passWord);  
             //注册成功，采用事件，直接通知文件服务，给用户自动添加一个头像。
            return Ok(result);
        }
        [HttpGet]
        [Route("test")]
        public  IActionResult Test()
        {
            return new JsonResult(new { Name = "张三" });
        }
            
    }
}