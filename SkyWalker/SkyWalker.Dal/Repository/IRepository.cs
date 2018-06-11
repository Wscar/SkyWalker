﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyWalker.Dal.Repository
{
   public interface IRepository<T> where T:Entities.Entity
    {
        Task< T> GetAsync(int id);
        Task< int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeltetAsync(T entity);
    }
}
