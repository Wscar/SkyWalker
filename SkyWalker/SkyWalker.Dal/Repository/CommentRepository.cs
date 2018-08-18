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
    /*
     * 评论这一块设计不合理，以后重构，暂时维持现在的使用方法。
     */ 
    public class CommentRepository : ICommentRepository
    {
        private readonly MySqlConnection conn;
        public CommentRepository(MySqlConnection _conn)
        {
            conn = _conn;
        }
        public async Task<int> AddAsync(Comment entity)
        {
            var sql = @"insert  into skywalker.`comment`(owner_user_id,target_user_id,story_id, content,create_time)
                     values(@owner_id,@target_user_id,@story_id,@content,@create_time)";
            var param = new
            {
                user_id = entity.OwnerUserId,
                target_user_id = entity.TargetUserId,
                story_id = entity.StoryId,
                entity.Content,
                create_time = DateTime.UtcNow
            };


            var result = await conn.ExecuteAsync(sql, param);
            return result;

        }

        public async Task<int> DeltetAsync(Comment entity)
        {
            string sql = "delete a from skywalker.comment a where a.id=@id";
            var param = new { id = entity.Id };
            var result = await conn.ExecuteAsync(sql, param);
            return result;

        }
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetAllAsync(object Id)
        {
            //应该更具故事Id去获取所有的评论
            string sql = @"SELECT
	                            a.id,
	                            a.owner_user_id ownerUserId,
	                            a.target_user_id targeUserId,
	                            a.story_id storyId,
	                            a.content,
	                            a.create_time createTime,
	                            a.update_time updateTime,
	                            b.UserName ownerUserName
                            FROM
	                            skywalker.`comment` a,
	                            skywalker.AppUser b
                            WHERE
	                            a.owner_user_id = b.Id
                            AND a.story_id=@id";
            var param = new { id = Id };
            var result = (await conn.QueryAsync<Comment>(sql, param)).ToList();
            return result;
        }
        /// <summary>
        /// 获取单条评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Comment> GetAsync(object id)
        {
            //应该是从ownUserId来查询
            string sql = @"select  a.id,
	                            a.owner_user_id ownerUserId,
	                            a.target_user_id targeUserId,
	                            a.story_id storyId,
	                            a.content,
	                            a.create_time createTime,
	                            a.update_time updateTime
                            from  skywalker.`comment` a 
                            where a.owner_user_id=@id";
            var param = new { id };
            var result = (await conn.QueryAsync<Comment>(sql, conn)).ToList();
            if (result.Count > 0)
            {
                return result.FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 更新评论
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Comment entity)
        {
            string sql = @"UPDATE skywalker.`comment` a
                            SET a.content = @content,
                             a.update_time = NOW()
                            WHERE
	                            a.id =@id ";
            var param = new { id = entity.Id, content = entity.Content };
            var result = await conn.ExecuteAsync(sql, param);
            return result;
        }
    }
}
