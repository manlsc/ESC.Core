using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 业务伙伴
    /// </summary>
    public class BBusinessPartnerRepository : BaseRepository<BBusinessPartner>
    {

        #region 构造

        public BBusinessPartnerRepository() : base(){}

        public BBusinessPartnerRepository(string connectionString) : base(connectionString){}

        public BBusinessPartnerRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = @"SELECT  BP.* ,
                                        CE.EnumDesc AS BusinessPartnerTypeName ,
                                        CE1.EnumDesc AS InUseName ,
                                        CE2.EnumDesc AS IsInnerName ,
                                        O.OrgName ,
                                        CU.UserName CreateByUserName ,
                                        UU.UserName UpdateByUserName
                                FROM    BBusinessPartner BP
                                        LEFT JOIN SCommonEnum CE ON BP.BusinessPartnerType = CE.EnumField AND CE.EnumType = 'BusinessPartnerType'
                                        LEFT JOIN SCommonEnum CE1 ON BP.InUse = CE1.EnumField AND CE.EnumType = 'InUse'
                                        LEFT JOIN SCommonEnum CE2 ON BP.IsInner = CE2.EnumField AND CE.EnumType = 'Bool'
                                        LEFT JOIN SOrganization O ON BP.OrgID = O.ID
                                        LEFT JOIN SUser CU ON BP.CreateBy = CU.ID
                                        LEFT JOIN SUser UU ON BP.UpdateBy = UU.ID";
            return searchSql;
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (WhereItem item in whereItems)
            {
                item.field = "BP." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
            return " ORDER BY BP.BusinessPartnerCode ASC";
        }
        #endregion

        #region 操作

        /// <summary>
        /// 根据id查询业务伙伴
        /// </summary>
        /// <param name="bpId"></param>
        /// <returns></returns>
        public BBusinessPartner GetBusinessPartnerById(int bpId)
        {
            string sqlSelect = GetSearchSql() + " WHERE BP.ID=" + bpId;
            return FirstOrDefault(sqlSelect);
        }

        /// <summary>
        /// 判断业务伙伴是否存在
        /// </summary>
        /// <param name="bPartner"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(BBusinessPartner bPartner, bool isAdd)
        {
            string sql = "SELECT BusinessPartnerCode FROM BBusinessPartner WHERE BusinessPartnerCode='" + bPartner.BusinessPartnerCode + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + bPartner.ID;
            }
            string rest = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rest);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeOrName"></param>
        /// <returns></returns>
        public List<BBusinessPartner> GetBusinessPartnerByCodeName(string codeOrName)
        {
            string sql = string.Format("SELECT TOP 2 T.* FROM BBusinessPartner T WITH(NOLOCK) WHERE t.BusinessPartnerCode LIKE '%{0}%' OR T.BusinessPartnerName  LIKE '%{0}%'", codeOrName);
            return Query(sql);
        }
        #endregion
    }
}
