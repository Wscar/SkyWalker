using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyWalker.Dal.Entities;
using Microsoft.AspNetCore.JsonPatch;
using SkyWalker.Dal.Repository;
using User.API.Dtos;
using User.API.Exceptions;
using System.IO;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : BaseController
    {
        private readonly IStoryRepository storyRepository;
        public StoryController(IStoryRepository _storyRepository)
        {
            storyRepository = _storyRepository;
        }

        [HttpGet]
        [Route("{bookId}")]
        public async Task<IActionResult> GetAlllStory(int bookId)
        {
            StoryDto storyDto = new StoryDto();
            var stories= await storyRepository.GetAllStoryAsync(bookId);
            if(stories !=null && stories.Count>0)
            {
                storyDto.Status = EntityStatus.Suceess;
                storyDto.Stories = stories;
            }
            else
            {
                var ex = new SkyWalkerException($"获取所有故事出错bookId:{bookId}");
                throw ex;
            }
            return Ok(stories);
        }
        [HttpGet]
        [Route("story/{storyId}:int")]
        public async Task<IActionResult> GetStory(int storyId)
        {
            var story = await storyRepository.GetStoryAsync(storyId);
            StoryDto storyDto = new StoryDto();
            if (story != null)
            {
                storyDto.Status = EntityStatus.Suceess;
                storyDto.Stories = new List<Story> { story };
            }
            else
            {
                var ex = new SkyWalkerException($"获取故事出错storyId:{storyId}");
            }
            return Ok(story);
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddStory(Story story)
        {
            StoryDto storyDto = new StoryDto();
            var result = await storyRepository.AddStoryAsync(story);
            if (result)
            {
                storyDto.Status = EntityStatus.Suceess;
            }
            else
            {
                storyDto.Status = EntityStatus.Fail;
                storyDto.Message = "添加故事失败,受影响的行数小于0";
            }
            return Ok(storyDto);
        }
        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> UpdateStory([FromBody] JsonPatchDocument<Story> patch,int storyId)
        {
            StoryDto storyDto = new StoryDto();
            //获取故事数据
            var story = await storyRepository.GetStoryAsync(storyId);
            patch.ApplyTo(story);
            var result = await storyRepository.UpdateStoryAsync(story);
            if (result)
            {
                storyDto.Status = EntityStatus.Suceess;
                storyDto.Stories = new List<Story> { story };
            }
            else
            {
                storyDto.Status = EntityStatus.Fail;
                storyDto.Message = $"更新故事失败,故事Id:{storyId},受影响的行数小于0";
            }
            return Ok(storyDto);
        }
        [HttpDelete]
        [Route("{storyId}:int")]
        public async Task<IActionResult> DeleteStory(int storyId)
        {
            StoryDto storyDto = new StoryDto();
            var result = await storyRepository.DeleteStoryAsync(storyId);
            if (result)
            {
                storyDto.Status = EntityStatus.Suceess;
               
            }
            else
            {
                storyDto.Status = EntityStatus.Fail;
                storyDto.Message = $"删除故事出错，故事Id{storyId},受影响的行数小于等于0";
            }
            return Ok(storyDto);
        }
        [HttpPost]
        [Route("uploadimage")]
        public async Task<IActionResult> UpLoadImage([FromForm] List<IFormFile> files)
        {
            var filePath = @"C:\Users\夜莫白\Desktop\图片接口.jpg";
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            //给前端返回fileName
            return Ok(new { status = "文件上传完成", filePath });
        }
    }
}