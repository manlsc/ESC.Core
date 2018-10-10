using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
    public class SErrorLogRepository : BaseRepository<SErrorLog>
    {
        #region 构造

        public SErrorLogRepository()
            : base()
        {
        }

        public SErrorLogRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SErrorLogRepository(DatabaseContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            string sqlStr = "SELECT EL.* FROM SErrorLog EL WITH (NOLOCK)";
            foreach (WhereItem item in whereItems)
            {
                item.field = "EL." + item.field;
            }
            return sqlStr + BulidWhereSql(whereItems) + " ORDER BY EL.ID DESC";
        }
    }
}
