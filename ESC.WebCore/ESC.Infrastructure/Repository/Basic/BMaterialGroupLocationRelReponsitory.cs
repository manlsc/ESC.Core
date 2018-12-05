using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 物料租-存储单元关系
    /// </summary>
    public class BMaterialGroupLocationRelRepository : BaseRepository<BMaterialGroupLocationRel>
    {

        #region 构造

        public BMaterialGroupLocationRelRepository() : base(){}

        public BMaterialGroupLocationRelRepository(string connectionString) : base(connectionString){}

        public BMaterialGroupLocationRelRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        /// <summary>
        /// 根据物料组ID查询存储单元
        /// </summary>
        /// <param name="materialGroupId"></param>
        /// <returns></returns>
        public List<BMaterialGroupLocationRel> GetBMaterialGroupLocationByMaterialGroupId(int materialGroupId)
        {
            string sql= "SELECT * FROM BMaterialGroupLocationRel WHERE MaterialGroupID=" + materialGroupId;
            return Query(sql);
        }
    }
}
