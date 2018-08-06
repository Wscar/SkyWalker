using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyWalker.Dal.Repository
{
   public  interface IFileRepository
    {
        Task<object> UpLoadFileAsync(string fileType,byte[] fileBytes);
        Task<byte[]> DownLoadFileAsync(string fileName);
        Task<byte[]> DownLoadFileAsync(object Id);
        Task<object> UpdateFileAsync(string fileName, byte[] fileBytes);
        Task<object> UpdateFileAsync(object Id, byte[] fileBytes);
        Task<object> DeleteFiledAsync(string fileName);
        Task<object> DeleteFiledAsync(object Id);
        Task<object> RenameFileAsync(object Id,string newFileName);
        Task<object> RenameFileAsync(string fileName,string newFileName);

    }
}
