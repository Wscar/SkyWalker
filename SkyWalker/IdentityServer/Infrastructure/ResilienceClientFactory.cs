using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Resilience.HttpInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using System.Net.Http;

namespace IdentityServer.Infrastructure
{
    public class ResilienceClientFactory
    {
        private readonly ILogger<ResilienceHttpClient> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// 重试次数
        /// </summary>
        private int retryCount;
        /// <summary>
        /// 熔断之前异常的次数
        /// </summary>
        private int exceptionAllowBeforeBreaking;
        public ResilienceClientFactory(ILogger<ResilienceHttpClient> _logger,IHttpContextAccessor _httpContextAccessor,int _retryCount, int _exceptionAllowBeforeBreaking)
        {
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
            retryCount = _retryCount;
            exceptionAllowBeforeBreaking = _exceptionAllowBeforeBreaking;
        }
        public ResilienceHttpClient GetResilienceHttpClient() =>
            new ResilienceHttpClient((origin)=>CreatePolicy(origin),logger,httpContextAccessor);
        private Policy[] CreatePolicy(string origin)
        {
            return new Policy[]
            {
                Policy.Handle<HttpRequestException>()
                //重试
                .WaitAndRetryAsync(retryCount,retryAttemp=>TimeSpan.FromSeconds( Math.Pow(2,retryAttemp)),(exception,timeSpan,retryCount,context)=>{
                    var msg=$"第{retryCount}次重试"+
                    $"of{context.PolicyKey}"+
                    $"at{context.OperationKey},"+
                    $"due to{exception}.";
                   logger.LogWarning(msg);
                    logger.LogDebug(msg);
                }),
                //熔断
                Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(exceptionAllowBeforeBreaking,
                TimeSpan.FromMinutes(1),
                ((exception,duration)=>{ logger.LogWarning("熔断器打开");
                    logger.LogDebug("熔断器打开");
                }),
                (()=>{

                 logger.LogWarning("熔断器关闭");
                 logger.LogDebug("熔断器关闭");}))
            };
        }
    } 
}
