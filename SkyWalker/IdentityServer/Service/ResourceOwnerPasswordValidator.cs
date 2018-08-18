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
            //待加入redis，对登陆请求进行控制，用户不能频繁的登陆。
            var accountResult = await accountService.SignInAsync(context.UserName, context.Password);         
            if (accountResult.Status=="登陆成功")
            {
                context.Result= new GrantValidationResult(accountResult.User.Id.ToString(), "admin", GetUserClaim(accountResult.User));
            }
           
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, accountResult.Message);
            }
        }
        public Claim[] GetUserClaim(UserInfo userInfo)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("userId", userInfo.UserId));
            claims.Add(new Claim("userName", userInfo.UserName));
            claims.Add(new Claim("avatar", userInfo.Avatar??"无"));
            claims.Add(new Claim("sex", userInfo.Sex.ToString()??"0"));
            claims.Add(new Claim("id", userInfo.Id.ToString()));
            return claims.ToArray();           
        }
        //var claims=  new Claim[] { new Claim("USERID",userInfo.UserId),new Claim("USERNAME",userInfo.UserName),
        //                      new Claim("USERPASSWORD",userInfo.UserPassWord),new Claim("AVATAR",userInfo.Avatar??"无"),
        //                      new Claim("SEX",userInfo.Sex.ToString()??"无"),new Claim("DESCRIBE",userInfo.Describe??"无"),
        //                       new Claim("PHONE",userInfo.Phone??"无"),new Claim("BIRTHDAY",userInfo.Brithday.ToString("yyyy-MM-dd HH:mm:ss")??DateTime.Now.ToShortDateString())};
    }
}
