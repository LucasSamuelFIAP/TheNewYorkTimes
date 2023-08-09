using Microsoft.EntityFrameworkCore;
using TheNewYorkTimes.Interfaces.Repositories;

namespace TheNewYorkTimes.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly SqlContext _sqlContext;

        public BaseRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task Add(TEntity obj)
        {
            try
            {
                _sqlContext.Set<TEntity>().Add(obj);
                await _sqlContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _sqlContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _sqlContext.Set<TEntity>().FindAsync(id);
        }

        public async Task Remove(int id)
        {
            try
            {
                var obj = await GetById(id);

                if (obj != null)
                {
                    _sqlContext.Set<TEntity>().Remove(obj);
                    await _sqlContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(TEntity obj)
        {
            try
            {
                _sqlContext.Entry(obj).State = EntityState.Modified;
                await _sqlContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
