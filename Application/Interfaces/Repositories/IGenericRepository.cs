﻿
namespace Application.Repository
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GetByIdAsync(int id);
        Task UpdateAsync(Entity entity, int id);
        Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties);
    }
}