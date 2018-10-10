using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    public class WOtherInLineRepository : BaseRepository<WOtherInLine>
    {

        #region 构造

        public WOtherInLineRepository() : base() { }

        public WOtherInLineRepository(string connectionString) : base(connectionString) { }

        public WOtherInLineRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = @"SELECT  SI.* ,
                                        WH.LocationDesc AS WarehouseName ,
                                        P.LocationDesc AS PositionName ,
                                        M.MaterialName AS MaterialName ,
                                        U.UnitName ,
                                        CU.UserName AS CreateByUserName ,
                                        UU.UserName AS UpdateByUserName
                                FROM    WOtherInLine SI WITH ( NOLOCK )
                                        LEFT JOIN BLocation WH WITH ( NOLOCK ) ON SI.WarehouseID = WH.ID
                                        LEFT JOIN BLocation P WITH ( NOLOCK ) ON SI.PositionID = P.ID
                                        LEFT JOIN BMaterial M WITH ( NOLOCK ) ON SI.MaterialID = M.ID
                                        LEFT JOIN BUnit U WITH ( NOLOCK ) ON SI.UnitID = U.ID
                                        LEFT JOIN SUser CU WITH ( NOLOCK ) ON SI.CreateBy = CU.ID
                                        LEFT JOIN SUser UU WITH ( NOLOCK ) ON SI.UpdateBy = UU.ID";
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
                item.field = "SI." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public string GetOrderBy()
        {
            return " ORDER BY SI.ID DESC";
        }

        #endregion

        /// <summary>
        ///根据主表删除明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int RemoveLinesByParentId(int parentId)
        {
            string sql = "DELETE FROM WOtherInLine WHERE ParentID=" + parentId;
            return Execute(sql);
        }

        /// <summary>
        /// 根据主表查询明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WOtherInLine> GetLinesByParentId(int parentId)
        {
            string sql = "SELECT * FROM WOtherInLine WITH(NOLOCK) WHERE ParentID=" + parentId;
            return Query(sql);
        }

        /// <summary>
        /// 根据盘点单添加盘亏出库
        /// </summary>
        /// <param name="otherIn"></param>
        public int AddOtherLinesByInv(WOtherIn otherIn, int invId)
        {
            string sql = @"INSERT INTO WOtherInLine
                                    ( WarehouseID ,
                                      PositionID ,
                                      WarehouseCode ,
                                      PositionCode ,
                                      MaterialID ,
                                      MaterialCode ,
                                      UnitID ,
                                      Batch ,
                                      OwnerCode ,
                                      Factory ,
                                      CreateDate ,
                                      CreateBy ,
                                      UpdateDate ,
                                      UpdateBy ,
                                      InCount ,
                                      ParentID
                                    )
                        SELECT      WarehouseID ,
                                    PositionID ,
                                    WarehouseCode ,
                                    PositionCode ,
                                    MaterialID ,
                                    MaterialCode ,
                                    UnitID ,
                                    Batch ,
                                    OwnerCode ,
                                    Factory ,
			                        GETDATE() AS CreateDate,
			                        {0} AS CreateBy,
			                        GETDATE() AS UpdateDate,
			                        0 AS UpdateBy,
			                        InventoryDiff AS InCount,
			                        {1} AS ParentID
                        FROM WInventoryLine WITH(NOLOCK) WHERE ParentID={2} AND InventoryDiff>0";
            return Execute(string.Format(sql, otherIn.CreateBy, otherIn.ID, invId));
        }
    }
}
