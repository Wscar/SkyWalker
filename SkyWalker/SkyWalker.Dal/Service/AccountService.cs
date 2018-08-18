using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;
using SkyWalker.Dal.Dtos;
using SkyWalker.Dal.Entities;
using System.Linq;
using SkyWalker.Dal.Repository;

namespace SkyWalker.Dal.Service
{
    public class AccountService : IAccountService
    {
        private readonly MySqlConnection conn;
        private readonly UserRepository userRepository;
        public AccountService(MySqlConnection _conn)
        {
            conn = _conn;
            userRepository = new UserRepository(conn);
        }

        public async Task<AccountResult> RegisterAsync(string userId, string passWord,string userName)
        {
            string sql = @"insert skywalker.AppUser(userId,userPassWord,userName)
                           values(@userId, @passWord,@userName); ";
            var accountResult = new AccountResult();
            try
            {
                var param = new { userId, passWord ,userName};
                var result = await conn.ExecuteAsync(sql, param);
                if (result > 0)
                {
                    //查到完成的用户信息
                    var user = await userRepository.GetAsync(userId);
                    accountResult.User = user;
                    accountResult.Status = "注册成功";
                }              
            }
            catch (Exception ex)
            {
                accountResult.Status = "注册失败";
                accountResult.Message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return accountResult;
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
            }
            catch (Exception ex)
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
