using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyWalker.Dal.Entities
{
    public class AppUser:Entity
    {
       

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserId {get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassWord { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 0男，1女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
        

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Describe { get; set; }
             
        public DateTime Brithday { get; set; }
    }
}
