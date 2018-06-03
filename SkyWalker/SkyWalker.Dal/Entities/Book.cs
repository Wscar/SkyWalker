using System;
using System.Collections.Generic;
using System.Text;

namespace SkyWalker.Dal.Entities
{
   public class Book
    {   
        /// <summary>
        /// 书籍ID
        /// </summary>
        public int BookId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; set; }
        /// <summary>
        /// 序言
        /// </summary>
        public string Preface { get; set; }
        /// <summary>
        /// 状态(公开/私密)
        /// </summary>
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
