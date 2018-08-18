using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MySql.Data.MySqlClient;
using FluentAssertions;
using SkyWalker.Dal.Service;
using User.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SkyWalker.Dal.Dtos;

namespace UserTest
{
  public  class AccountControllerTest
    {
        private string strConn = "server=47.93.232.105;uid=yemobai;pwd=Jolly1128@;database=skywalker;Charset=utf8;port=3306";
        public AccountController GetController(MySqlConnection conn)
        {
            IAccountService accountService = new AccountService(conn);
            var controller = new AccountController(accountService, null);
            return controller;
        }
        [Fact]
        public async void Post_Signin_WithExpectedParmaerters()
        {
            using(var conn=new MySqlConnection(strConn))
            {
                var controller = this.GetController(conn);
                var response=await controller.SignIn("yemobai", "wqawd520");
                var result = response.Should().BeOfType<OkObjectResult>().Subject;
                var value = result.Value.Should().BeAssignableTo<AccountResult>().Subject;
                value.Status.Should().Be("登陆成功");
            }
        }
    }
}
