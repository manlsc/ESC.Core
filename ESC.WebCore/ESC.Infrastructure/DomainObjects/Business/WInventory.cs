using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    public class WInventory : PocoObject
    {

        public WInventory()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
            Lines = new List<WInventoryLine>();
        }

        /// <summary>
        /// 盘点单号
        /// </summary>
        [Column("InventoryCode","盘点单号",true,true,true,160,"")]
        public string InventoryCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("InventoryStatus", "状态", true, true, true, 160, "InventoryStatusName")]
        public int InventoryStatus { set; get; }

        /// <summary>
        /// 盘亏单号
        /// </summary>
        [Column("OtherOutID", "盘亏单号", false, true, true, 160, "")]
        public string OtherOutID { get; set; }
        
        /// <summary>
        /// 盘亏单号
        /// </summary>
        [Column("OtherOutCode", "盘亏单号", true, true, true, 160, "")]
        public string OtherOutCode { get; set; }

        /// <summary>
        /// 盘盈单号
        /// </summary>
        [Column("OtherInID", "盘盈单号", false, true, true, 160, "")]
        public string OtherInID { get; set; }

        /// <summary>
        /// 盘盈单号
        /// </summary>
        [Column("OtherInCode", "盘盈单号", true, true, true, 160, "")]
        public string OtherInCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, true, true, 160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, true, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", true, true, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", true, true, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ResultColumn]
        public string CreateByUserName { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [ResultColumn]
        public string UpdateByUserName { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [ResultColumn]
        public string InventoryStatusName { get; set; }

        [Ignore]
        public List<WInventoryLine> Lines { set; get; }

    }
}
