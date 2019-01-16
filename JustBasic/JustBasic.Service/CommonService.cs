using JustBasic.Common;
using JustBasic.Data.Infrastructure;
using JustBasic.Data.Repositories;
using JustBasic.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace JustBasic.Service
{
    public interface ICommonService
    {
        Footer GetFooter();

        Footer GetConfig(string Name);

        IEnumerable<Slide> GetSlides();

        SystemConfig GetSystemConfig(string code);
    }

    public class CommonService : ICommonService
    {
        private IFooterRepository _footerRepository;
        private ISystemConfigRepository _systemConfigRepository;
        private IUnitOfWork _unitOfWork;
        private ISlideRepository _slideRepository;

        public CommonService(IFooterRepository footerRepository, ISystemConfigRepository systemConfigRepository, IUnitOfWork unitOfWork, ISlideRepository slideRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
        }

        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.Name == CommonConstants.DefaultFooterId);
        }

        public Footer GetConfig(string name)
        {
            return _footerRepository.GetSingleByCondition(x => x.Name == name);
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x => x.Status).OrderBy(x => x.DisplayOrder);
        }

        public SystemConfig GetSystemConfig(string code)
        {

            var rs =  _systemConfigRepository.GetSingleByCondition(x => x.Code.Trim() == code.Trim());
            //var rs = _systemConfigRepository.GetConfig(code);
            return rs;
        }
    }
}