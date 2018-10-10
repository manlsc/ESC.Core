using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    public class BLocationUserRelRepository : BaseRepository<BLocationUserRel>
    {

        #region 构造

        public BLocationUserRelRepository() : base(){}

        public BLocationUserRelRepository(string connectionString) : base(connectionString){}

        public BLocationUserRelRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 操作

        /// <summary>
        /// 根据仓位查询库管员
        /// </summary>
        /// <param name="loctionId"></param>
        /// <returns></returns>
        public List<BLocationUserRel> GetLocUserRelsByLocId(int loctionId)
        {
            return Query("SELECT * FROM BLocationUserRel WHERE LocationID=" + loctionId);
        }

        #endregion
    }
}
