using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    public class WTransferOut : PocoObject
    {

        public WTransferOut()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
            Lines = new List<WTransferOutLine>();
        }

        /// <summary>
        /// TransferOutCode
        /// </summary>
        [Column("TransferOutCode", "出库单号", true, true, true, 160, "")]
        public string TransferOutCode { get; set; }

        /// <summary>
        /// FWarehouseID
        /// </summary>
        [Column("FWarehouseID", "调出仓库", true, true, false, 160, "FWarehouseName")]
        public int FWarehouseID { get; set; }

        /// <summary>
        /// FWarehouseCode
        /// </summary>
        [Column("FWarehouseCode", "调出仓库编码", true, true, true, 160, "")]
        public string FWarehouseCode { get; set; }

        /// <summary>
        /// TWarehouseID
        /// </summary>
        [Column("TWarehouseID", "调入仓库", true, true, false, 160, "TWarehouseName")]
        public int TWarehouseID { get; set; }

        /// <summary>
        /// TWarehouseCode
        /// </summary>
        [Column("TWarehouseCode", "调入仓库编码", true, true, true, 160, "")]
        public string TWarehouseCode { get; set; }

        /// <summary>
        /// 出库类型
        /// </summary>
        [Column("StockOutType", "出库类型", false, true, false, 160, "StockOutTypeName")]
        public int StockOutType { get; set; }

        /// <summary>
        /// 业务伙伴
        /// </summary>
        [Column("BusinessPartnerID", "业务伙伴", true, true, false, 160, "BusinessPartnerName")]
        public int BusinessPartnerID { get; set; }

        /// <summary>
        /// 出库状态
        /// </summary>
        [Column("StockStatus", "出库状态", true, true, true, 160, "StockStatusName")]
        public int StockStatus { set; get; }

        /// <summary>
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, true, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, true, true, 160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", true, true, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", true, true, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

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
        /// 入库类型
        /// </summary>
        [ResultColumn]
        public string StockOutTypeName { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        [ResultColumn]
        public string StockStatusName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [ResultColumn]
        public string FWarehouseName { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        [ResultColumn]
        public string TWarehouseName { set; get; }

        [Ignore]
        public List<WTransferOutLine> Lines { set; get; }

    }
}
