using JustBasic.Data.Infrastructure;
using JustBasic.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBasic.Data.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {

    }
    public class TagRepository : RepositoryBase<Tag>,ITagRepository
    {
        public TagRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
