using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly ILogger<CorsPolicyService> logger;
        public CorsPolicyService(ILogger<CorsPolicyService> _logger)
        {
            logger = _logger;
        }
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
          
            var task= Task.Run<bool>(() =>
            {
                if(origin== "http://localhost:8080")
                {
                    return true;
                }
                return false;
            });
            return task;
        }
    }
}
