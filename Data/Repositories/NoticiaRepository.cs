using TheNewYorkTimes.Interfaces.Repositories;
using TheNewYorkTimes.Models;

namespace TheNewYorkTimes.Data.Repositories
{
    public class NoticiaRepository : BaseRepository<Noticia>, INoticiaRepository
    {
        private readonly SqlContext _sqlContext;

        public NoticiaRepository(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }
    }
}
