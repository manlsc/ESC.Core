using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    public class WSellNoticeLineRepository : BaseRepository<WSellNoticeLine>
    {

        #region 构造

        public WSellNoticeLineRepository() : base(){}

        public WSellNoticeLineRepository(string connectionString) : base(connectionString){}

        public WSellNoticeLineRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 语句

        public string GetSearchSql()
        {
             string searchSql = @"  SELECT  T1.* ,
                                            T2.MaterialName ,
                                            T3.UnitName ,
                                            T4.UserName AS CreateByUserName ,
                                            T5.UserName AS UpdateByUserName
                                    FROM    WSellNoticeLine T1 WITH ( NOLOCK )
                                            LEFT JOIN BMaterial T2 WITH ( NOLOCK ) ON T1.MaterialID = T2.ID
                                            LEFT JOIN BUnit T3 WITH ( NOLOCK ) ON T1.UnitID = T3.ID
                                            LEFT JOIN SUser T4 WITH ( NOLOCK ) ON T1.CreateBy = T4.ID
                                            LEFT JOIN SUser T5 WITH ( NOLOCK ) ON T1.UpdateBy = T5.ID";
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
        /// 添加下推
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public decimal AddDownCount(decimal downCount, int lineId)
        {
            string sql = "UPDATE WSellNoticeLine SET DownCount=DownCount+" + downCount + " OUTPUT Inserted.OutCount-Inserted.DownCount WHERE ID=" + lineId;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 减少下推
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public decimal RemoveDownCount(decimal downCount, int lineId)
        {
            string sql = "UPDATE WSellNoticeLine SET DownCount=DownCount-" + downCount + " OUTPUT Inserted.DownCount WHERE ID=" + lineId;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 更新下推数量
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="outCount"></param>
        /// <param name="inputCount">更新数量</param>
        /// <returns></returns>
        public decimal UpdateDownCount(int lineId, decimal outCount, decimal inputCount)
        {
            //如果两个数量相同,不更新
            if (inputCount > outCount)
            {
                decimal count = inputCount - outCount;
                string sql = "UPDATE WSellNoticeLine SET DownCount=DownCount-" + count + " OUTPUT Inserted.DownCount WHERE ID=" + lineId;
                return Execute(sql);
            }
            else
            {
                decimal count = outCount - inputCount;
                string sql = "UPDATE WSellNoticeLine SET DownCount=DownCount+" + count + " OUTPUT Inserted.OutCount-Inserted.DownCount WHERE ID=" + lineId;
                return Execute(sql);
            }
        }

        /// <summary>
        /// 添加完成 减少下推
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public decimal AddCompleteCount(int lineId, decimal count)
        {
            string sql = "UPDATE WSellNoticeLine SET CompleteCount=CompleteCount+" + count + ",DownCount=DownCount-" + count + "  OUTPUT Inserted.OutCount-Inserted.CompleteCount WHERE ID=" + lineId;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 是否全部入库
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsAllDownload(int parentId)
        {
            string sql = "SELECT TOP 1 ID FROM WSellNoticeLine WITH(NOLOCK) WHERE ParentID=" + parentId + " AND OutCount>DownCount";
            string result = ExecuteScalar<string>(sql);
            return string.IsNullOrEmpty(result);
        }

        /// <summary>
        ///根据主表删除明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int RemoveLinesByParentId(int parentId)
        {
            string sql = "DELETE FROM WSellNoticeLine WHERE ParentID=" + parentId;
            return Execute(sql);
        }

        /// <summary>
        /// 根据主表查询明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WSellNoticeLine> GetLinesByParentId(int parentId)
        {
            string sql = "SELECT * FROM WSellNoticeLine WITH(NOLOCK) WHERE ParentID=" + parentId;
            return Query(sql);
        }
    }
}
