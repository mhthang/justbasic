using JustBasic.Data.Infrastructure;
using JustBasic.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace JustBasic.Data.Repositories
{
    public interface IPriceRepository : IRepository<Price>
    {
        IEnumerable<Price> GetPriceProduct(int ProductID);

        Price GetPrice(int productID, int sizeID, int colorId);

        Price GetPriceById(int ID);

        Price GetByPrimary(int ProductID);
    }

    public class PriceRepository : RepositoryBase<Price>, IPriceRepository
    {
        public PriceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Price> GetPriceProduct(int ProductID)
        {
            var query = from p in DbContext.Prices
                        where p.ProductID == ProductID
                        orderby p.SizeID descending
                        select p;
            return query;
        }

        public Price GetPriceById(int ID)
        {
            var query = from p in DbContext.Prices
                        where p.ID == ID
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public Price GetByPrimary(int productID)
        {
            var query = from p in DbContext.Prices
                        where p.ProductID == productID && p.IsPrimary
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }

        public Price GetPrice(int productID, int sizeID, int colorId)
        {
            var query = from p in DbContext.Prices
                        where p.ProductID == productID && p.SizeID == sizeID && p.ColorID == colorId
                        orderby p.ID descending
                        select p;
            return query.FirstOrDefault();
        }
    }
}