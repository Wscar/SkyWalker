using SkyWalker.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyWalker.Dal.Service
{
  public  interface IAccountService
    {
        Task<AccountResult> SignInAsync(string userId, string passWord);
        Task<AccountResult> SignInByPhoneAsync(string phone, string passWord,string validationCode);
        Task<AccountResult> RegisterAsync(string userId, string passWord,string userName);
    }
}
