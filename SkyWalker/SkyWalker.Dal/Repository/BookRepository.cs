using System;
using System.Collections.Generic;
using System.Text;
using SkyWalker.Dal.Repository;
using SkyWalker.Dal.Entities;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;
namespace SkyWalker.Dal.Repository
{
    public class BookRepository : IRepository<Book>
    {
        private readonly MySqlConnection connection;
        public BookRepository(MySqlConnection _connection)
        {
            connection = _connection;
        }
        public async Task<int> AddAsync(Book entity)
        {
            string sql = "insert into skywalker.book(user_id,book_name,priface,status,create_time,update_time)"
                        + "values(@user_id,@book_name,@priface,@status,@create_time,@update_time)";
            var param = new { user_id = entity.UserId, book_name = entity.BookName, priface = entity.Preface, status = entity.Status, create_time = entity.CreateTime, update_time = entity.UpdateTime };
            return await connection.ExecuteAsync(sql, param);
        }

        public async Task<int> DeltetAsync(Book entity)
        {
            string sql = "delete from skywalker.book a where a.id=@id";
            var param = new { id=entity.Id };
            return await connection.ExecuteAsync(sql, param);
        }

        public async Task<Book> GetAsync(int id)
        {
            string sql = "select * from skywalker.book a where a.id=@id";
            var param =  new { id };
            return (await connection.QueryAsync<Book>(sql, param)).FirstOrDefault();
        }

        public async Task<int> UpdateAsync(Book entity)
        {
            string sql = "update  skywalker.book a" +
                 "       set a.book_name=@book_name"
                        + " set a.priface=@priface"
                        + " set a.status=@status"
                        + " set a.update_time=@update_time"
                        + " where id=@id";
            var param = new { id = entity.Id };
            return await connection.ExecuteAsync(sql, param);
        }
    }
}
