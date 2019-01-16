using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System.Collections.Generic;

namespace JustBasic.Service
{
    public interface ISlideService
    {
        void Add(Slide Slide);

        void Update(Slide Slide);

        Slide Delete(int id);

        IEnumerable<Slide> GetAll();

        IEnumerable<Slide> GetAllPaging(int page, int pageSlide, out int totalRow);

        Slide GetById(int id);

        void SaveChanges();
    }

    public class SlideService : ISlideService
    {
        private ISlideRepository SlideRepository;
        private IUnitOfWork _unitOfWork;

        public SlideService(ISlideRepository SlideRepository, IUnitOfWork unitOfWork)
        {
            this.SlideRepository = SlideRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Slide Slide)
        {
            SlideRepository.Add(Slide);
        }

        public Slide Delete(int id)
        {
            return SlideRepository.Delete(id);
        }

        public IEnumerable<Slide> GetAll()
        {
            return SlideRepository.GetAll(new string[] { });
        }

        public IEnumerable<Slide> GetAllPaging(int page, int pageSlide, out int totalRow)
        {
            return SlideRepository.GetMultiPaging(x => x.ID > 0, out totalRow, page, pageSlide);
        }

        public Slide GetById(int id)
        {
            return SlideRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Slide Slide)
        {
            SlideRepository.Update(Slide);
        }
    }
}