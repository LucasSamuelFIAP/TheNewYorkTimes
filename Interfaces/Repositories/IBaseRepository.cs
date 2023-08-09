namespace TheNewYorkTimes.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity obj);

        Task Update(TEntity obj);

        Task Remove(int id);

        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(int id);
    }
}
