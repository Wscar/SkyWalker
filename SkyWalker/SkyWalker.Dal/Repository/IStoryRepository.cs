using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SkyWalker.Dal.Entities;
namespace SkyWalker.Dal.Repository
{
    public interface IStoryRepository
    {
        Task<Story> GetStoryAsync(int storyId);
        Task<List<Story>> GetAllStoryAsync(int bookId);
        Task<bool> AddStoryAsync(Story story);
        Task<bool> UpdateStoryAsync(Story story);
        Task<bool> DeleteStoryAsync(int storyId);
    }
}
