using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;
using SkyWalker.Dal.Dtos;
using SkyWalker.Dal.Entities;
using System.Linq;
namespace SkyWalker.Dal.Service
{
    public class AccountService : IAccountService
    {
        private readonly MySqlConnection conn;
        public AccountService(MySqlConnection _conn)
        {
            conn = _conn;
        }
        public async Task<AccountResult> SignInAsync(string userId, string passWord)
        {
            var accountResult = new AccountResult();
            try
            {   
                //判断用户名是否存在
                string sql = @"select  * from skywalker.AppUser a
                         where a.UserId=@userId";
                var param = new { userId };
           
                var user = (await conn.QueryAsync<AppUser>(sql, param)).FirstOrDefault();
             
                if (user.UserId == userId && user.UserPassWord == passWord)
                {
                    accountResult.Status = "登陆成功";
                    accountResult.User = user;
                }
                else
                {                 
                    accountResult.Status = "登陆失败";
                    accountResult.Message = "用户名密码错误";
                }
            }catch(Exception ex)
            {
                accountResult.Status = "登陆失败";
                accountResult.Message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
           
            return accountResult;
        }

        public async Task<AccountResult> SignInByPhoneAsync(string phone, string passWord, string validationCode)
        {
            throw new NotImplementedException();
        }
    }
}
