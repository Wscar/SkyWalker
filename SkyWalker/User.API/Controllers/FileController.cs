using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Consumes("application/json", "multipart/form-data")]
    public class FileController : ControllerBase
    {
        [HttpPost]
        [Route("uploadimage")]     
        [AllowAnonymous]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> UpLoadImage([FromForm]List<IFormFile> files)
        {
            long size = files.Sum(x => x.Length);
            var filePath = Path.GetTempPath();
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
            return Ok(new { status = "文件上传完成", filePath });
        }
        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok("可以访问");
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var factories = context.ValueProviderFactories;
            factories.RemoveType<FormValueProviderFactory>();
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}