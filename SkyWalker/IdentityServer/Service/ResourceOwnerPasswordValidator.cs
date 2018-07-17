using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class ResourceOwnerPasswordValidator: IResourceOwnerPasswordValidator
    {
        private readonly IAccountService accountService;
         public ResourceOwnerPasswordValidator(IAccountService _accountService)
        {
            accountService = _accountService;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var accountResult = await accountService.SignInAsync(context.UserName, context.Password);
            if (accountResult.Status == "查询成功")
            {
                context.Result= new GrantValidationResult(accountResult.User.Id.ToString(), "admin", GetUserClaim());
            }
        
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "密码错误");
            }
        }
        public Claim[] GetUserClaim()
        {
            return new Claim[] { new Claim("userid", "1"), new Claim("avatar", "无") };
        }
    }
}
