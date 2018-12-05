using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 出库明细
    /// </summary>
    public class WStockOutRepository : BaseRepository<WStockOut>
    {

        #region 构造

        public WStockOutRepository() : base(){}

        public WStockOutRepository(string connectionString) : base(connectionString){}

        public WStockOutRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion
    }
}
