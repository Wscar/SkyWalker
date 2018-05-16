using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace User.API.IdentityServerValidator
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "yemobai" && context.Password == "123")
            {
                context.Result = new GrantValidationResult(context.UserName, "admin", GetUserClaim());

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
