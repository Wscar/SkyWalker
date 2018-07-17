using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyWalker.Dal.Repository;
using SkyWalker.Dal.Entities;
using Microsoft.AspNetCore.JsonPatch;
using User.API.Dtos;
using User.API.Exceptions;
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
            BookDto bookDto = new BookDto();
            var book=  await bookRepository.GetAsync(id);
            if(book != null)
            {
                bookDto.Status = EntityStatus.Suceess;
                bookDto.Books = new List<Book>() { book };
                return Ok(bookDto);
            }
            else
            {

                var ex = new SkyWalkerException($"错误的书籍id:{id}");
                throw ex;
                
            }
         
        }
        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> Pacth([FromBody] JsonPatchDocument<Book> patch,int bookId)
        {
            BookDto bookDto = new BookDto();
            var book = await bookRepository.GetAsync(bookId);
            patch.ApplyTo(book);
            var result= await bookRepository.UpdateAsync(book);
            if (result > 0)
            {
                bookDto.Status = EntityStatus.Suceess;
                bookDto.Books = new List<Book>() { book };
                return Ok(bookDto);
            }
            else
            {
                bookDto.Status = EntityStatus.Fail;
                bookDto.Message = "更新书籍失败";
                return Ok(bookDto);
            }
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            BookDto bookDto = new BookDto();
            var result=  await bookRepository.AddAsync(book);
            if (result > 0)
            {
                bookDto.Status = EntityStatus.Suceess;
                bookDto.Books = new List<Book>() { book };
                return Ok(bookDto);
            }
            else
            {
                bookDto.Status = EntityStatus.Fail;
                bookDto.Message = "添加数据失败";
                return Ok(bookDto);
            }
        }
        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var result = await bookRepository.DeltetAsync(new Book { Id = bookId });
            BookDto bookDto = new BookDto();
            if (result > 0)
            {
                bookDto.Status = EntityStatus.Suceess;
                return Ok(bookDto);
            }
            else
            {
                bookDto.Status = EntityStatus.Fail;
                bookDto.Message = "删除数据失败";
                return Ok(bookDto);
            }
        }
        [HttpGet]
        [Route("all_book/{userId}")]
        public async Task<IActionResult> GetAllBook(int userId)
        {
            BookDto bookDto = new BookDto();
            var books = await bookRepository.GetAllAsync(userId);
            if (books != null && books.Count > 0)
            {
                bookDto.Status = EntityStatus.Suceess;
                bookDto.Books = books;
               
            }
            else
            {
                var ex = new SkyWalkerException($"获取所有数据失败userId:{userId}");
                throw ex;
            }
            return Ok(bookDto);
        }
    }
}