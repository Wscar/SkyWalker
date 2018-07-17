using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Exceptions;
namespace User.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment env;
        private readonly ILogger<GlobalExceptionFilter> logger;
        public GlobalExceptionFilter(IHostingEnvironment _env, ILogger<GlobalExceptionFilter> _logger)
        {
            env = _env;
            logger = _logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(SkyWalkerException))
            {
                var json = new JsonErrResponse
                {
                    Message = context.Exception.Message

                };
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                var json = new JsonErrResponse() { Message = "发生了未知的内部异常"};
                if (env.IsDevelopment())
                {
                    json.DeveloerMessage = context.Exception.Message;
                }
                context.Result = new InternalServerErroObjectResult(json);
            }
            //记录日志
            logger.LogError(context.Exception, context.Exception.Message);
          
        }
    }
    public class InternalServerErroObjectResult : ObjectResult
    {
        public InternalServerErroObjectResult(object err) : base(err)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
