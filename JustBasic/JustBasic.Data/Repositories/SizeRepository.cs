using JustBasic.Data.Infrastructure;
using JustBasic.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBasic.Data.Repositories
{
    public interface ISizeRepository : IRepository<Size>
    {
        IEnumerable<Size> GetSizeByProduct(int ProductID);

        IEnumerable<Size> GetSizeByNotPrice(int ProductID);
    }
    public class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        public SizeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Size> GetSizeByNotPrice(int ProductID)
        {
            var query = from p in DbContext.Prices
                        where p.ProductID == ProductID
                        orderby p.SizeID descending
                        select p.Size;
            return query;
        }

        public IEnumerable<Size> GetSizeByProduct(int ProductID)
        {
            var query = from s in DbContext.Sizes
                        where !DbContext.Prices.Any(f => f.SizeID == s.ID)
                        select s;

            return query;
        }
    }
}
