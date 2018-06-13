using System;
using System.Collections.Generic;
using System.Text;
using SkyWalker.Dal.Entities;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
namespace SkyWalker.Dal.Repository
{
    public class UserRepository : IRepository<AppUser>
    {
        private readonly MySqlConnection connection;
        public UserRepository(MySqlConnection _connection)
        {
            connection = _connection;
        }

        public async Task<int> AddAsync(AppUser entity)
        {
            string sql = "insert into appuser(userid,avatar,sex,username,userpassword,phone,describe,brithday)" +
                       "value(@id,@avatar,@sex,@username,@userpassword,@phone,@describe,@brithday)   ";
            var param = new
            {
                id = entity.UserId,
                avatr = entity.Avatar,
                username = entity.UserName,
                userpassword = entity.UserPassWord,
                phone = entity.Phone,
                describe = entity.Describe,
                brithday = entity.Brithday
            };
            return await connection.ExecuteAsync(sql, param);

        }

        public async Task<int> DeltetAsync(AppUser entity)
        {
            string sql = "delete from appuser where userid=@id";
            var param = new { id = entity.Id };
            return await connection.ExecuteAsync(sql, param);
        }

        public Task<List<AppUser>> GetAllAsync(string  userId)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetAsync(int id)
       {
            string sql = "select * from AppUser a where  a.userid=@id";
            var param = new { id };
             var result= await connection.QueryAsync<AppUser>(sql, param);
            return result.FirstOrDefault();
        }

        public async Task<int> UpdateAsync(AppUser entity)
        {
            string sql = "update appuser set userpassword=@password" +
                       " set username=@name set avatar=@avatar set phone=@phone" +
                       " set describe=@describe set brithday=@brithday" +
                       " where userid=@id          ";
            var param = new
            {
                name = entity.UserName,
                avatar = entity.Avatar,
                phone = entity.Phone,
                describe = entity.Describe,
                brithday = entity.Brithday,
                id = entity.UserId
            };
            return await connection.ExecuteAsync(sql, param);
        }
    }
}
