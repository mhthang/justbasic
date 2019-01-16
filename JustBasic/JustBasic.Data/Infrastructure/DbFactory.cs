using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBasic.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private JustBasicDbContext dbContext;

        public JustBasicDbContext Init()
        {
            return dbContext ?? (dbContext = new JustBasicDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
