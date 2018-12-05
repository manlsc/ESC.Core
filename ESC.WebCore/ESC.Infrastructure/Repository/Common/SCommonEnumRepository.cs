using ESC.Infrastructure.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 枚举 +
    /// </summary>
    public class SCommonEnumRepository : BaseRepository<SCommonEnum>
    {

        #region 构造

        public SCommonEnumRepository()
            : base()
        {
        }

        public SCommonEnumRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SCommonEnumRepository(DatabaseContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            string sql= "SELECT C.* FROM SCommonEnum C WITH (NOLOCK)";
            foreach (WhereItem item in whereItems)
            {
                item.field = "C." + item.field;
            }
            return sql + BulidWhereSql(whereItems) + " ORDER BY C.EnumType ASC";
        }

        /// <summary>
        /// 根据字段查询配置
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public List<SCommonEnum> GetCommonEnumByField(string field)
        {
            return Query("SELECT * FROM SCommonEnum WITH (NOLOCK) WHERE EnumField='" + field + "'");
        }

        public List<SCommonEnum> GetCommonEnum(string field, string enumType)
        {
            return Query("SELECT * FROM SCommonEnum WITH (NOLOCK) WHERE EnumField='" + field + "' AND EnumType='" + enumType + "'");
        }

        public List<SCommonEnum> GetCommonEnumByType(string enumType)
        {
            return Query("SELECT * FROM SCommonEnum WITH (NOLOCK) WHERE EnumType='" + enumType + "'");
        }
    }
}
