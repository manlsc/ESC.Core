using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    public class WSellLineRepository : BaseRepository<WSellLine>
    {

        #region 构造

        public WSellLineRepository() : base() { }

        public WSellLineRepository(string connectionString) : base(connectionString) { }

        public WSellLineRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        public string GetSearchSql()
        {
            string searchSql = @"SELECT  T1.* ,
                                            T2.LocationDesc AS PositionName ,
                                            T3.MaterialName ,
                                            T4.UnitName ,
                                            T5.UserName AS CreateByUserName ,
                                            T6.UserName AS UpdateByUserName
                                    FROM    WSellLine T1 WITH ( NOLOCK )
                                            LEFT JOIN BLocation T2 WITH ( NOLOCK ) ON T1.PositionID = T2.ID
                                            LEFT JOIN BMaterial T3 WITH ( NOLOCK ) ON T1.MaterialID = T3.ID
                                            LEFT JOIN BUnit T4 WITH ( NOLOCK ) ON T1.UnitID = T4.ID
                                            LEFT JOIN SUser T5 WITH ( NOLOCK ) ON T1.CreateBy = T5.ID
                                            LEFT JOIN SUser T6 WITH ( NOLOCK ) ON T1.UpdateBy = T6.ID";
            return searchSql;
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (var item in whereItems)
            {
                item.field = "T1." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
            return " ORDER BY T1.ID DESC";
        }


        #endregion

        /// <summary>
        /// 根据父类查询明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WSellLine> GetLinesByParentId(int parentId)
        {
            string sql = GetSearchSql() + " WHERE T1.ParentID=" + parentId;
            return Query(sql);
        }

        /// <summary>
        ///根据主表删除明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int RemoveLinesByParentId(int parentId)
        {
            string sql = "DELETE FROM WSellLine WHERE ParentID=" + parentId;
            return Execute(sql);
        }

        /// <summary>
        /// 添加完成
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public decimal AddReturnCount(decimal returnCount, int lineId)
        {
            string sql = "UPDATE WSellLine SET ReturnCount=ReturnCount+" + returnCount + " OUTPUT Inserted.OutCount-Inserted.ReturnCount WHERE ID=" + lineId;
            return ExecuteScalar<decimal>(sql);
        }


        /// <summary>
        /// 减少退库
        /// </summary>
        /// <param name="returnCount"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public decimal RemoveReturnCount(decimal returnCount, int lineId)
        {
            string sql = "UPDATE WSellLine SET ReturnCount=ReturnCount-" + returnCount + " OUTPUT Inserted.ReturnCount WHERE ID=" + lineId;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 更新下推数量
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="outCount"></param>
        /// <param name="inputCount">更新数量</param>
        /// <returns></returns>
        public decimal UpdateReturnCount(int lineId, decimal outCount, decimal inputCount)
        {
            //如果两个数量相同,不更新
            if (inputCount > outCount)
            {
                decimal count = inputCount - outCount;
                string sql = "UPDATE WSellLine SET ReturnCount=ReturnCount-" + count + " OUTPUT Inserted.ReturnCount WHERE ID=" + lineId;
                return ExecuteScalar<decimal>(sql);
            }
            else
            {
                decimal count = outCount - inputCount;
                string sql = "UPDATE WSellLine SET ReturnCount=ReturnCount+" + count + " OUTPUT Inserted.OutCount-Inserted.ReturnCount WHERE ID=" + lineId;
                return ExecuteScalar<decimal>(sql);
            }
        }
    }
}
