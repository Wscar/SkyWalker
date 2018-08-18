using System;
using System.Collections.Generic;
using System.Text;

namespace SkyWalker.Dal.Entities
{
   public class Comment : Entity
    {   
      
        /// <summary>
        /// 发表评论的用户ID
        /// </summary>
        public int OwnerUserId { get; set; }

        public string OwnerUserName { get; set; }
        /// <summary>
        /// 评论的目标用户ID
        /// </summary>
        public int TargetUserId { get; set; }
        /// <summary>
        /// 故事ID
        /// </summary>
        public int StoryId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 创建评论的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
