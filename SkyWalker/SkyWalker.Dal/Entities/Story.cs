using System;
using System.Collections.Generic;
using System.Text;

namespace SkyWalker.Dal.Entities
{
  public  class Story
    {   /*
        每本书包含n个故事
         */
        public int Id { get; set; }
        /// <summary>
        /// 书籍ID
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverPhoto { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
