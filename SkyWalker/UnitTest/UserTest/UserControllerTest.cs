using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using FluentAssertions;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SkyWalker.Dal.Entities;
using User.API.Controllers;
using SkyWalker.Dal.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace UserTest
{   
    public class UserControllerTest
    {
        private readonly SkyWalkerDbContext dbContext;
        public UserControllerTest()
        {
            dbContext = this.GetDbContext();
        }
        private SkyWalkerDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SkyWalkerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var dbContext = new SkyWalkerDbContext(options);
            dbContext.AppUsers.Add(new AppUser()
            {
                Id = 1,
                UserId = "yemobai",
                UserName = "夜莫白",
                UserPassWord = "123"

            });
            dbContext.Add(new AppUser()
            {
                Id = 2,
                UserId = "123",
                UserName = "123",
                UserPassWord = "123"
            });
            dbContext.SaveChanges();
            return dbContext;
        }
        private UserController GetUserController()
        {
            var controller = new UserController(dbContext,null);
            return controller;
        }
        [Fact]
        public async void Get_ReturnRightUser_WithExpectedParmaerters()
        {
            var controller = GetUserController();
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id==1);
            user = dbContext.AppUsers.Where(x => x.Id==1).FirstOrDefault();
            var response = await controller.GetUserAsync(user.Id);
            //fluent方法
            //拿到返回值
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var jsonValue = result.Value.Should().BeAssignableTo<Dictionary<string, AppUser>>().Subject;
            jsonValue.Should().NotBeNull();
            jsonValue.Should().ContainKey("success");
            jsonValue["success"].Should().BeOfType<AppUser>().Subject.Id.Should().Be(1);
        }
        [Fact]
        public async void Patch_ReturnRightUser_WithExpectedParameter()
        {
            var controller = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(x => x.UserName, "Jolly" );
            var resonse = await controller.UpdateUserAsync(document);
            // 进行返回值验证
            var result = resonse.Should().BeOfType<JsonResult>().Subject;
            var jsonValue = result.Value.Should().BeAssignableTo<Dictionary<string, AppUser>>().Subject;
            jsonValue.Should().NotBeNull();
            jsonValue.Should().ContainKey("success");
            jsonValue["success"].Should().BeOfType<AppUser>().Subject.Id.Should().Be(1);
            jsonValue["success"].Should().BeOfType<AppUser>().Subject.UserName.Should().Be("Jolly");
        }
        [Fact]
        public async void Delete_ReturnRightValue_WithExpectedParameter()
        {
            var controller = GetUserController();
            var response=await controller.DeleteUserAsync(2);
            var result= response.Should().BeOfType<JsonResult>().Subject;
            var jsonValue = result.Value.Should().BeAssignableTo<Dictionary<string, bool>>().Subject;
            jsonValue.Should().NotBeNull();
            jsonValue.Should().ContainKey("del_user");
            jsonValue["del_user"].Should().BeTrue();
        }

    }
}
