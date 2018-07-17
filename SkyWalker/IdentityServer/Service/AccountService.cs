using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Dtos;

namespace IdentityServer.Service
{
    public class AccountService : IAccountService
    {
        public Task<AccountResult> SignInAsync(string userId, string passWord)
        {
            throw new NotImplementedException();
        }

        public Task<AccountResult> SignInByPhoneAsync(string phone, string passWord, string validationCode)
        {
            throw new NotImplementedException();
        }
    }
}
