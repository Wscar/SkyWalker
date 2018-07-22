using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Dtos;
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
            if (accountResult.Status=="登陆成功")
            {
                context.Result= new GrantValidationResult(accountResult.User.Id.ToString(), "admin", GetUserClaim(accountResult.User));
            }       
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "密码错误");
            }
        }
        public Claim[] GetUserClaim(UserInfo userInfo)
        {
            //var claims=  new Claim[] { new Claim("USERID",userInfo.UserId),new Claim("USERNAME",userInfo.UserName),
            //                      new Claim("USERPASSWORD",userInfo.UserPassWord),new Claim("AVATAR",userInfo.Avatar??"无"),
            //                      new Claim("SEX",userInfo.Sex.ToString()??"无"),new Claim("DESCRIBE",userInfo.Describe??"无"),
            //                       new Claim("PHONE",userInfo.Phone??"无"),new Claim("BIRTHDAY",userInfo.Brithday.ToString("yyyy-MM-dd HH:mm:ss")??DateTime.Now.ToShortDateString())};
            var claims = new Claim[] { new Claim("USERID", userInfo.UserId), new Claim("USERNAME", userInfo.UserName) };
            return claims;
        }
    }
}
