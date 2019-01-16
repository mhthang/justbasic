using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace JustBasic.Service
{
    public interface IColorService
    {
        void Add(Color Color);

        void Update(Color Color);

        Color Delete(int id);

        IEnumerable<Color> GetAll();

        IEnumerable<Color> GetAllPaging(int page, int pageSize, out int totalRow);

        Color GetById(int id);

        IEnumerable<Color> GetAllByProduct(int ProductID);

        void SaveChanges();
    }

    public class ColorService : IColorService
    {
        private IColorRepository _colorRepository;
        private IUnitOfWork _unitOfWork;

        public ColorService(IColorRepository ColorRepository, IUnitOfWork unitOfWork)
        {
            this._colorRepository = ColorRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Color Color)
        {
            _colorRepository.Add(Color);
        }

        public Color Delete(int id)
        {
            return _colorRepository.Delete(id);
        }

        public IEnumerable<Color> GetAll()
        {
            return _colorRepository.GetAll(new string[] { });
        }

        public IEnumerable<Color> GetAllByProduct(int ProductID)
        {
            return _colorRepository.GetColorByProduct(ProductID).Distinct();
        }

        public IEnumerable<Color> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _colorRepository.GetMultiPaging(x => x.ID > 0, out totalRow, page, pageSize);
        }

        public Color GetById(int id)
        {
            return _colorRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Color Color)
        {
            _colorRepository.Update(Color);
        }
    }
}