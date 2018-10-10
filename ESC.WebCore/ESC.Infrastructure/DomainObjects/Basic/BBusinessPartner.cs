using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    public class BBusinessPartner : PocoObject
    {
        public BBusinessPartner()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 业务伙伴编码
        /// </summary>
        [Column("BusinessPartnerCode", "伙伴编码", true, true, false, 160, "")]
        public string BusinessPartnerCode { get; set; }

        /// <summary>
        /// 业务伙伴名称
        /// </summary>
        [Column("BusinessPartnerName", "伙伴名称", true, true, false, 160, "")]
        public string BusinessPartnerName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Column("BusinessPartnerType", "类型", true, true, false, 160, "BusinessPartnerTypeName")]
        public int BusinessPartnerType { get; set; }

        /// <summary>
        /// 地区代码
        /// </summary>
        [Column("CountryCode", "地区代码", false, false, false, 160, "")]
        public string CountryCode { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Column("City", "城市", false, false, false, 160, "")]
        public string City { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Column("Address", "地址", true, false, false, 160, "")]
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Column("ZipCode", "邮编", true, false, false, 160, "")]
        public string ZipCode { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column("Mail", "邮箱", true, false, false, 160, "")]
        public string Mail { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Column("Mobile", "电话", true, false, false, 160, "")]
        public string Mobile { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [Column("Fax", "传真", true, false, false, 160, "")]
        public string Fax { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        [Column("Websit", "网址", false, false, false, 160, "")]
        public string Websit { get; set; }

        /// <summary>
        /// 财务地址
        /// </summary>
        [Column("FinanceAddress", "财务地址", false, false, false, 160, "")]
        public string FinanceAddress { get; set; }

        /// <summary>
        /// 财务电话
        /// </summary>
        [Column("FinanceMobile", "财务电话", false, false, false, 160, "")]
        public string FinanceMobile { get; set; }

        /// <summary>
        /// 账户组
        /// </summary>
        [Column("AccountGroup", "账户组", false, false, false, 160, "")]
        public string AccountGroup { get; set; }

        /// <summary>
        /// 是否内部单位
        /// </summary>
        [Column("IsInner", "是否内部单位", false, false, false, 160, "IsInnerName")]
        public int IsInner { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        [Column("OrgID", "组织机构", true, false, false, 160, "OrgName")]
        public int OrgID { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Column("InUse", "是否可用", false, false, false, 160, "InUseName")]
        public int InUse { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, false, true, 160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, false, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", false, false, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", false, false, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark", "备注", false, false, false, 160, "")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [ResultColumn]
        public string CreateByUserName { set; get; }

        /// <summary>
        /// 更新人名称
        /// </summary>
        [ResultColumn]
        public string UpdateByUserName { set; get; }

        /// <summary>
        /// 类型
        /// </summary>
        [ResultColumn]
        public string BusinessPartnerTypeName { get; set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        [ResultColumn]
        public string InUseName { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        [ResultColumn]
        public string OrgName { get; set; }

        /// <summary>
        /// 是否内部单位
        /// </summary>
        [ResultColumn]
        public string IsInnerName { get; set; }

    }
}
