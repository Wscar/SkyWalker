using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SkyWalker.Dal.Entities;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq;
namespace SkyWalker.Dal.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly MySqlConnection connection;
        public StoryRepository(MySqlConnection _connection)
        {
            connection = _connection;
        }
        public async Task<bool> AddStoryAsync(Story story)
        {
            string sql = @"insert into skywalker.story(book_id,
                            user_id,
                            title,
                            content,
                            cover_photo,
                            create_time,
                            update_time)
                            value(@book_id,
                            @user_id,
                            @title,
                            @content,
                            @cover_photo,
                            @create_time,
                            @update_time)";
            var param = new
            {
                book_id = story.BookId,
                user_id = story.UserId,
                title = story.UserId,
                content = story.Content,
                cover_photo = story.CoverPhoto,
                create_time = story.CreateTime,
                update_time = story.UpdateTime
            };
            int result = await connection.ExecuteAsync(sql, param);
            return result > 0;
        }

        public async Task<bool> DeleteStoryAsync(int storyId)
        {
            string sql = @"delete a from skywalker.story a where a.id=@storyId ";
            var param = new { storyId };
            int result = await connection.ExecuteAsync(sql, param);
            return result > 0;
        }

        public async Task<List<Story>> GetAllStoryAsync(int bookId)
        {
            string sql = @"select a.id Id,a.book_id bookId,
                        a.user_id userId,
                        a.title,
                        a.content ,
                        a.cover_photo coverPhoto,
                        a.create_time createTime,
                        a.update_time updateTime
                          from skywalker.story as a 
                        where a.book_id=@book_id";
            var param = new { book_id = bookId };
            return (await connection.QueryAsync<Story>(sql, param)).ToList();
        }

        public async Task<Story> GetStoryAsync(int storyId)
        {
            string sql = @"select a.id Id,a.book_id bookId,
                        a.user_id userId,
                        a.title,
                        a.content ,
                        a.cover_photo coverPhoto,
                        a.create_time createTime,
                        a.update_time updateTime
                          from skywalker.story as a 
                        where a.id=@storyId";
            var param = new { storyId };
            return (await connection.QueryAsync<Story>(sql, param)).FirstOrDefault();
        }

        public async Task<bool> UpdateStoryAsync(Story story)
        {
            string sql = @"update  skywalker.story a
                            set a.title=@title,
                            a.content=@content,
                            a.cover_photo=@cover_photo,
                            a.update_time=@update_time
                            where a.id=@Id";
            var param = new { story.Id, title = story.Title, content = story.Content, cover_photo = story.CoverPhoto, update_time = story.UpdateTime };
            var result = await connection.ExecuteAsync(sql, param);
            return result > 0;

        }
    }
}
