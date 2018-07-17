using IdentityServer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public interface IAccountService
    {
        Task<AccountResult> SignInAsync(string userId, string passWord);
        Task<AccountResult> SignInByPhoneAsync(string phone, string passWord, string validationCode);
    }
}
