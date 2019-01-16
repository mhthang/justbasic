using JustBasic.Data.Infrastructure;
using JustBasic.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace JustBasic.Data.Repositories
{
    public interface IColorRepository : IRepository<Color>
    {
        IEnumerable<Color> GetColorByProduct(int ProductID);
    }

    public class ColorRepository : RepositoryBase<Color>, IColorRepository
    {
        public ColorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Color> GetColorByProduct(int ProductID)
        {
            var query = from p in DbContext.Prices
                        where p.ProductID == ProductID
                        orderby p.ColorID descending
                        select p.Color;
            return query;
        }
    }
}