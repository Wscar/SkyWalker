using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.ViewModel;
namespace User.API.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(SignInViewModel singInViewModel)
        {
            if (singInViewModel.UserName == "1")
            {
                return Content("我是1");
            }
            return Content("我不是1");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            return Content($"userId{viewModel.UserId},password={viewModel.Password},confirmPassword={viewModel.Password}");
        }
    }
}