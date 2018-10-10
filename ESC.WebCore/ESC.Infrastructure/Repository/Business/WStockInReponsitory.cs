using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    public class WStockInRepository : BaseRepository<WStockIn>
    {

        #region 构造

        public WStockInRepository() : base(){}

        public WStockInRepository(string connectionString) : base(connectionString){}

        public WStockInRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion
    }
}
