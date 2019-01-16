using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System.Collections.Generic;

namespace JustBasic.Service
{
    public interface IMenuService
    {
        void Add(Menu Menu);

        void Update(Menu Menu);

        Menu Delete(int id);

        IEnumerable<Menu> GetAll();

        IEnumerable<Menu> GetAllPaging(int page, int pageMenu, out int totalRow);

        Menu GetById(int id);

        void SaveChanges();
    }

    public class MenuService : IMenuService
    {
        private IMenuRepository MenuRepository;
        private IUnitOfWork _unitOfWork;

        public MenuService(IMenuRepository MenuRepository, IUnitOfWork unitOfWork)
        {
            this.MenuRepository = MenuRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Menu Menu)
        {
            MenuRepository.Add(Menu);
        }

        public Menu Delete(int id)
        {
            return MenuRepository.Delete(id);
        }

        public IEnumerable<Menu> GetAll()
        {
            return MenuRepository.GetAll(new string[] { });
        }

        public IEnumerable<Menu> GetAllPaging(int page, int pageMenu, out int totalRow)
        {
            return MenuRepository.GetMultiPaging(x => x.ID > 0, out totalRow, page, pageMenu);
        }

        public Menu GetById(int id)
        {
            return MenuRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Menu Menu)
        {
            MenuRepository.Update(Menu);
        }
    }
}