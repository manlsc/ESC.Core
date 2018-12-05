using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 行为日志 +
    /// </summary>
    public class SBehaviorLogRepository : BaseRepository<SBehaviorLog>
    {
        #region 构造

        public SBehaviorLogRepository()
            : base()
        {
        }

        public SBehaviorLogRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SBehaviorLogRepository(DatabaseContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            string sqlStr = "SELECT BL.* FROM SBehaviorLog BL WITH (NOLOCK)";
            foreach (WhereItem item in whereItems)
            {
                item.field = "BL." + item.field;
            }
            return sqlStr + BulidWhereSql(whereItems) + " ORDER BY BL.ID DESC";
        }
    }
}
