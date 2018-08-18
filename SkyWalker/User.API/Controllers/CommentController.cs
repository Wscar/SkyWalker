using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyWalker.Dal.Repository;
using User.API.Dtos;
namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        private readonly ICommentRepository commentRepository;
        public CommentController(ICommentRepository _commentRepository)
        {
            commentRepository = _commentRepository;
        }
        [HttpGet]
        [Route("{storyId:int}")]
        /// <summary>
        /// 获取这个故事下面的所有评论
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetAllCommentAsync(int storyId)
        {
            var result = await commentRepository.GetAllAsync(storyId);
            var commentDto = new CommentDto();
            if (result == null)
            {
                commentDto.Status = EntityStatus.Fail;
                commentDto.Message = "获取评论出错，详细信息请看日志";
                
            }
            commentDto.Status = EntityStatus.Suceess;
            commentDto.Commens = result;
            return Ok(commentDto);
        }
            
    }
}