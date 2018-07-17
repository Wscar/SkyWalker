using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkyWalker.Dal.Entities;
namespace User.API.Dtos
{
    public class BookDto:BaseDto
    {
        
        public List<Book> Books { get; set; }
    }
}
