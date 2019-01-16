using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace JustBasic.Service
{
    public interface IPriceService
    {
        void Add(Price Price);

        void Update(Price Price);

        Price Delete(int id);

        IEnumerable<Price> GetAll();

        IEnumerable<Price> GetAllPaging(int page, int pagePrice, out int totalRow);

        Price GetById(int id);

        Price GetByPrimary(int ProductID);

        IEnumerable<Price> GetPriceProduct(int ProductID);

        Price GetPrice(int ProductID, int sizeID, int colorId);

        void SaveChanges();
    }

    public class PriceService : IPriceService
    {
        private IPriceRepository _PriceRepository;
        private IUnitOfWork _unitOfWork;

        public PriceService(IPriceRepository PriceRepository, IUnitOfWork unitOfWork)
        {
            this._PriceRepository = PriceRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Price Price)
        {
            _PriceRepository.Add(Price);
        }

        public Price Delete(int id)
        {
            return _PriceRepository.Delete(id);
        }

        public IEnumerable<Price> GetAll()
        {
            return _PriceRepository.GetAll(new string[] { "Size", "Color", "Product" }).OrderBy(x => x.Product.Name);
        }

        public IEnumerable<Price> GetPriceProduct(int ProductID)
        {
            return _PriceRepository.GetPriceProduct(ProductID);
        }

        public IEnumerable<Price> GetAllPaging(int page, int pagePrice, out int totalRow)
        {
            return _PriceRepository.GetMultiPaging(x => x.ColorID > 0, out totalRow, page, pagePrice);
        }

        public Price GetById(int id)
        {
            return _PriceRepository.GetSingleById(id);
        }

        public Price GetByPrimary(int Product)
        {
            return _PriceRepository.GetByPrimary(Product);
        }

        public Price GetPrice(int ProductID, int sizeID, int colorId)
        {
            return _PriceRepository.GetPrice(ProductID, sizeID, colorId);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Price Price)
        {
            _PriceRepository.Update(Price);
        }
    }
}