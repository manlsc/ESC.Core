using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    public class WPurchaseReturn : PocoObject
    {

        public WPurchaseReturn()
        {
            Lines = new List<WPurchaseReturnLine>();
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 销售单号
        /// </summary>
        [Column("PurchaseReturnCode", "退库单号", true, true, true, 160, "")]
        public string PurchaseReturnCode { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        [Column("SourceCode", "来源单号", true, true, true, 160, "")]
        public string SourceCode { get; set; }


        [Column("SourceID", "SourceID", false, true, true, 160, "")]
        public int SourceID { set; get; }

        /// <summary>
        /// 仓库
        /// </summary>
        [Column("WarehouseID", "仓库", true, true, false, 160, "WarehouseName")]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        [Column("WarehouseCode", "仓库编码", true, true, true, 160, "")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 业务伙伴
        /// </summary>
        [Column("BusinessPartnerID", "业务伙伴", true, true, false, 160, "BusinessPartnerName")]
        public int BusinessPartnerID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("StockStatus", "状态", true, true, true, 160, "StockStatusName")]
        public int StockStatus { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, true, true, 160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, true, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", false, true, false, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", false, true, false, 160, "")]
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
        /// 业务伙伴
        /// </summary>
        [ResultColumn]
        public string BusinessPartnerName { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        [ResultColumn]
        public string StockStatusName { set; get; }

        /// <summary>
        /// 仓库
        /// </summary>
        [ResultColumn]
        public string WarehouseName { set; get; }

        [Ignore]
        public List<WPurchaseReturnLine> Lines { set; get; }
    }
}
