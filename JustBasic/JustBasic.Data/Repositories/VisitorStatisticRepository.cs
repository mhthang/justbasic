using JustBasic.Data.Infrastructure;
using JustBasic.Model.Models;

namespace JustBasic.Data.Repositories
{
    public interface IVisitorStatisticRepository : IRepository<VisitorStatistic>
    {
    }

    internal class VisitorStatisticRepository : RepositoryBase<VisitorStatistic>,IVisitorStatisticRepository
    {
        public VisitorStatisticRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}