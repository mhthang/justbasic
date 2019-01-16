using JustBasic.Data.Infrastructure;
using JustBasic.Model.Models;
using System.Linq;

namespace JustBasic.Data.Repositories
{
    public interface ISystemConfigRepository : IRepository<SystemConfig>
    {
        SystemConfig GetConfig(string code);
    }

    public class SystemConfigRepository : RepositoryBase<SystemConfig>, ISystemConfigRepository
    {
        public SystemConfigRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
        public SystemConfig GetConfig(string code)
        {
            //var rrr = this.DbContext.SystemConfigs.Where(x => x.Code == code);
            return this.DbContext.SystemConfigs.Where(x => x.Code == code).FirstOrDefault();
        }
    }
}