using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MySql.Data.MySqlClient;
using SkyWalker.Dal.Repository;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using User.API.Controllers;
using Dapper;
using SkyWalker.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;
namespace BookControllerTest
{
    public class ControllerTest
    {
        private readonly MySqlConnection connection;
        private readonly BookRepository bookRepository;

        public ControllerTest()
        {
            connection = new MySqlConnection(GetConfiguration().GetConnectionString("MySqlConnectionString"));
            bookRepository = new BookRepository(connection);
        }
        private IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            return config;
        }
        private BookController GetController()
        {
            var controller = new BookController(bookRepository);
            return controller;
        }
        private int GetTestUserId()
        {
            return 2;
        }
        [Fact]
        public async void Post_AddRightBook_WithExpectedParamerters()
        {
            var book = new Book
            {
                BookName = "在路上",
                Preface = "我于死亡中绽放，亦如黎明的花朵",
                Status = "OPEN",
                UserId = 2,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            var controller = this.GetController();
            var response = await controller.AddBook(book);
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            var value = result.Value.Should().BeAssignableTo<Book>().Subject;
            value.BookName.Should().Be("在路上");
        }
        [Fact]
        public async void Get_ReturnRightBook_WithExpectedParamerters()
        {
            var controller = this.GetController();
            var response = await controller.Get(1);
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            var value = result.Value.Should().BeAssignableTo<Book>().Subject;
            value.UserId.Should().Be(2);
        }
        [Fact]
        public async void Patch_ReturnRightBook_WithExpectedParamerters()
        {
            var controller = this.GetController();
            var document = new JsonPatchDocument<Book>();
            document.Replace(x => x.Preface, "长路慢慢，为剑所伴");
            var response = await controller.Pacth(document, 4);
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            var value = result.Value.Should().BeAssignableTo<Book>().Subject;
            value.UserId.Should().Be(2);
        }
        [Fact]
        public async void Delete_ReturnRightBook_WithExpectedParamerters()
        {
            var controller = this.GetController();
            var resonse = await controller.DeleteBook(1);
            var result = resonse.Should().BeOfType<OkObjectResult>().Subject;
            var value = result.Value.Should().BeAssignableTo<string>().Subject;
            value.Should().Be("删除成功");
        }
        [Fact]
        public async void Get_ReturnRightAllBook_WithExpectedParamerters()
        {
            var controller = this.GetController();
            var response = await controller.GetAllBook(2);
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            var value = result.Value.Should().BeAssignableTo<List<Book>>().Subject;
            value.First().UserId.Should().Be(2);
        }
    }
}
