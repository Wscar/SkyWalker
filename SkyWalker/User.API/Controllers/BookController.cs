using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyWalker.Dal.Repository;
using SkyWalker.Dal.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepository<Book> bookRepository;
        public BookController(IRepository<Book> _bookRepository)
        {
            bookRepository = _bookRepository;
        }
        [HttpGet]
        [Route("{id}")]
        public  async Task<IActionResult> Get(int id)
        {
           var book=  await bookRepository.GetAsync(id);
            if(book == null)
            {
                return Ok(book);
            }
            else
            {
                return Ok("没有获取到");
            }
           
        }
        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> Pacth([FromBody] JsonPatchDocument<Book> patch,int bookId)
        {
            var book = await bookRepository.GetAsync(bookId);
            patch.ApplyTo(book);
            var result= await bookRepository.UpdateAsync(book);
            if (result > 0)
            {
                return Ok(book);
            }
            else
            {
                return Ok("没有更新book的实体");
            }
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
           var result=  await bookRepository.AddAsync(book);
            if (result > 0)
            {
                return Ok(book);
            }
            else
            {
                return Ok("没有成功添加书籍");
            }
        }
        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var result = await bookRepository.DeltetAsync(new Book { Id = bookId });
            if (result > 0)
            {
                return Ok("删除成功");
            }
            else
            {
                return Ok("删除失败");
            }
        }
    }
}