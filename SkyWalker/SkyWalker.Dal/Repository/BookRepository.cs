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
            string sql = "insert into skywalker.book(user_id,book_name,preface,status,create_time,update_time)"
                        + "values(@user_id,@book_name,@priface,@status,@create_time,@update_time)";
            var param = new { user_id = entity.UserId, book_name = entity.BookName, priface = entity.Preface, status = entity.Status, create_time = entity.CreateTime, update_time = entity.UpdateTime };
            return await connection.ExecuteAsync(sql, param);
        }

        public async Task<int> DeltetAsync(Book entity)
        {
            string sql = "delete a from skywalker.book a where a.id=@id";
            var param = new { id = entity.Id };
            return await connection.ExecuteAsync(sql, param);
        }

        public async Task<List<Book>> GetAllAsync(object Id)
        {
            string sql = "select a.id, a.user_id userId,"
                        + "a.book_name bookName,"
                        + "a.preface ,"
                        + "a.status,"
                        + "a.create_time createTime,"
                        + "a.update_time updatetime"
                         + " from skywalker.book a where a.user_id = @userId";
            var param = new { userId=Id };
            return (await connection.QueryAsync<Book>(sql, param)).ToList();
        }

        public async Task<Book> GetAsync(object id)
        {
            string sql = "select a.id, a.user_id userId,"
                      + "a.book_name bookName,"
                      + "a.preface ,"
                      + "a.status,"
                      + "a.create_time createTime,"
                      + "a.update_time updatetime"
                       + " from skywalker.book a where a.id=@id";
            var param = new { id };
            var books = await connection.QueryAsync<Book>(sql, param);
            return books.FirstOrDefault();
        }
        public async Task<int> UpdateAsync(Book entity)
        {
            string sql = "update  skywalker.book a" +
                 "       set a.book_name=@book_name,"
                        + "  a.preface=@priface,"
                        + "  a.status=@status,"
                        + " a.update_time=@update_time"
                        + " where id=@id";
            var param = new
            {
                id = entity.Id,
                book_name = entity.BookName,
                priface = entity.Preface,
                status = entity.Status,
                update_time = entity.UpdateTime
            };
            return await connection.ExecuteAsync(sql, param);
        }
    }
}
