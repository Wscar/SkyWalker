using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkyWalker.Dal.Entities;
namespace User.API.Controllers
{
    public class BaseController : Controller
    {
        protected AppUser Identity
        {
            get
            {
                var user = new AppUser();
                user.Id = Convert.ToInt32(this.User.Claims.FirstOrDefault(x => x.Type == "id").Value);
                user.UserName = this.User.Claims.FirstOrDefault(x => x.Type == "userName").Value;
                user.Avatar = this.User.Claims.FirstOrDefault(x => x.Type == "avatar").Value;
                user.Sex = Convert.ToInt32(this.User.Claims.FirstOrDefault(x => x.Type == "sex").Value);
                user.UserId = this.User.Claims.FirstOrDefault(x => x.Type == "userId").Value;

                return user;
            }
        }
    }
}
