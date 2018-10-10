using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    public class WOtherIn : PocoObject
    {
        public WOtherIn()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
            Lines = new List<WOtherInLine>();
        }

        /// <summary>
        /// 入库单号
        /// </summary>
        [Column("OtherInCode", "入库单号", true, true, true, 160, "")]
        public string OtherInCode { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        [Column("StockInType", "入库类型", true, true, false, 160, "StockInTypeName")]
        public int StockInType { get; set; }

        /// <summary>
        /// 业务伙伴
        /// </summary>
        [Column("BusinessPartnerID", "业务伙伴", true, true, false, 160, "BusinessPartnerName")]
        public int BusinessPartnerID { get; set; }

        /// <summary>
        /// 入库状态
        /// </summary>
        [Column("StockStatus", "入库状态", true, true, true, 160, "StockStatusName")]
        public int StockStatus { set; get; }

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
        /// 业务伙伴
        /// </summary>
        [ResultColumn]
        public string BusinessPartnerName { set; get; }

        /// <summary>
        /// 入库类型
        /// </summary>
        [ResultColumn]
        public string StockInTypeName { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        [ResultColumn]
        public string StockStatusName { set; get; }

        [Ignore]
        public List<WOtherInLine> Lines { set; get; }
    }
}
