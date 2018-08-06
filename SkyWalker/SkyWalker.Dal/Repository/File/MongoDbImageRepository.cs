using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Linq;
namespace SkyWalker.Dal.Repository
{
   public class MongoDbImageRepository : IImageRepository
    {
        private readonly AppSetting appSetting;
        private IMongoDatabase database;
        private readonly ILogger<MongoDbImageRepository> logger;
        private GridFSBucket bucket;
        public MongoDbImageRepository(IOptions<AppSetting> options ,ILogger<MongoDbImageRepository> _logger)
        {
            appSetting = options.Value;
            logger = _logger;
        }
        private void Initialize()
        {             
            var client = new MongoClient(appSetting.MongonContactConnectionString);
            if (client != null)
            {
                database = client.GetDatabase(appSetting.MongoDbDatabaseSkyWalker);
                bucket = new GridFSBucket(database);
            }
        }
        private async Task<ObjectId> GetFileId(string fileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions
            {
                Limit = 1,
                Sort = sort
            };
             using(var cursor= await bucket.FindAsync(filter, options))
            {
                if (cursor != null)
                {
                    var fileInfo = cursor.ToList().FirstOrDefault();
                    return fileInfo.Id;
                }
               
            }
            return new ObjectId();
        }
        public async Task<object> UpLoadFileAsync(string fileType, byte[] imageBytes)
        {    
            //暂时采用guid来作为文件名，
            string fileName = $"{Guid.NewGuid().ToString()}.{fileType}";
            var id=   await bucket.UploadFromBytesAsync(fileName, imageBytes);
            //暂时不知道返回Id如何保存金mysql数据库
            return fileName;
        }

        public async Task<byte[]> DownLoadFileAsync(string fileName)
        {
            //var id = await GetFileId(fileName);
            var options = new GridFSDownloadOptions
            {

            };
            var bytes = await bucket.DownloadAsBytesAsync(fileName);
            return bytes;
        }

        public async Task<byte[]> DownLoadFileAsync(object Id)
        {
            var objId =  (ObjectId) Id;
            if (objId.Timestamp!=0)
            {
                var bytes = await bucket.DownloadAsBytesAsync(objId);
                return bytes;
            }
            return null;
        }

        public async Task<object> UpdateFileAsync(string fileName, byte[] fileBytes)
        {
            var id= await  bucket.UploadFromBytesAsync(fileName, fileBytes);
            return id;
           
        }

        public  Task<object> UpdateFileAsync(object Id, byte[] fileBytes)
        {
            throw new NotImplementedException();

        }

        public Task<object> DeleteFiledAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<object> DeleteFiledAsync(object Id)
        {
            var fileId = (ObjectId)Id;
            await bucket.DeleteAsync(fileId);
            return true;
        }

        public async Task<object> RenameFileAsync(object Id, string newFileName)
        {
           var fileId= (ObjectId)Id;
            await bucket.RenameAsync(fileId,newFileName);
            return fileId;
        }

        public Task<object> RenameFileAsync(string fileName, string newFileName)
        {
            throw new NotImplementedException();
        }
    }
}
