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
using EventBus;
using User.API.TestCode;
using System.Text.RegularExpressions;

namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IEventBus eventBus;
        public AccountController(IAccountService _accountService, IEventBus _eventBus)
        {
            accountService = _accountService;
            eventBus = _eventBus;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string userId, string password, string userName)
        {
            var registerResult = new SkyWalker.Dal.Dtos.AccountResult();
            //判断用户Id不能为中文，密码不能为空,之后交由前端进行验证
            string pattern = "[\u4e00-\u9fbb]";
            bool regexResult = Regex.IsMatch(userId, pattern);
            if (regexResult)
            {
                registerResult.Status = "注册失败";
                registerResult.Message = "用户名不能包含中文";
            }
            else
            {
                registerResult = await accountService.RegisterAsync(userId, password, userName);
                //注册成功，采用事件，直接通知文件服务，给用户自动添加一个头像。
                await eventBus.PublishAsync(new FileEvent("123.text"));
            }
            return Ok(registerResult);
        }
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn(string userId, string passWord)
        {
            var result = await accountService.SignInAsync(userId, passWord);

            return Ok(result);
        }
        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return new JsonResult(new { Name = "张三" });
        }

    }
}