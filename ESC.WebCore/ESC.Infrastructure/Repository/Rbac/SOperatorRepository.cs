using ESC.Infrastructure.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
   public class SOperatorRepository:BaseRepository<SOperator>
    {
        #region 构造

        public SOperatorRepository() : base() { }

        public SOperatorRepository(string connectionString) : base(connectionString) { }

        public SOperatorRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion
    }
}
