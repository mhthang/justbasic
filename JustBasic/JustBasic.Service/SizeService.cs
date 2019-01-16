using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace JustBasic.Service
{
    public interface ISizeService
    {
        void Add(Size Size);

        void Update(Size Size);

        Size Delete(int id);

        IEnumerable<Size> GetAll();

        IEnumerable<Size> GetAllPaging(int page, int pageSize, out int totalRow);

        Size GetById(int id);

        IEnumerable<Size> GetAllByProduct(int ProductID);

        IEnumerable<Size> GetSizeByNotPrice(int ProductID);

        void SaveChanges();
    }

    public class SizeService : ISizeService
    {
        private ISizeRepository sizeRepository;
        private IUnitOfWork _unitOfWork;

        public SizeService(ISizeRepository SizeRepository, IUnitOfWork unitOfWork)
        {
            this.sizeRepository = SizeRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Size Size)
        {
            sizeRepository.Add(Size);
        }

        public Size Delete(int id)
        {
            return sizeRepository.Delete(id);
        }

        public IEnumerable<Size> GetAll()
        {
            return sizeRepository.GetAll(new string[] { });
        }

        public IEnumerable<Size> GetAllByProduct(int ProductID)
        {
            return sizeRepository.GetSizeByProduct(ProductID).Distinct();
        }

        public IEnumerable<Size> GetSizeByNotPrice(int ProductID)
        {
            return sizeRepository.GetSizeByNotPrice(ProductID).Distinct();
        }

        public IEnumerable<Size> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return sizeRepository.GetMultiPaging(x => x.ID > 0, out totalRow, page, pageSize);
        }

        public Size GetById(int id)
        {
            return sizeRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Size Size)
        {
            sizeRepository.Update(Size);
        }
    }
}