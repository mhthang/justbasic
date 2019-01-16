using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System.Collections.Generic;

namespace JustBasic.Service
{
    public interface IFooterService
    {
        void Add(Footer Footer);

        void Update(Footer Footer);

        Footer Delete(int id);

        IEnumerable<Footer> GetAll();

        Footer GetById(int id);

        void SaveChanges();
    }

    public class FooterService : IFooterService
    {
        private IFooterRepository sizeRepository;
        private IUnitOfWork _unitOfWork;

        public FooterService(IFooterRepository FooterRepository, IUnitOfWork unitOfWork)
        {
            this.sizeRepository = FooterRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Footer Footer)
        {
            sizeRepository.Add(Footer);
        }

        public Footer Delete(int id)
        {
            return sizeRepository.Delete(id);
        }

        public IEnumerable<Footer> GetAll()
        {
            return sizeRepository.GetAll(new string[] { });
        }

        public Footer GetById(int id)
        {
            return sizeRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Footer Footer)
        {
            sizeRepository.Update(Footer);
        }
    }
}