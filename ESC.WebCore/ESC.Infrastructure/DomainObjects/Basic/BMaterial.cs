using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    public class BMaterial : PocoObject
    {
        public BMaterial()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        [Column("MaterialCode", "物料编码", true, true, false, 160, "")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [Column("MaterialName", "物料名称", true, true, false, 160, "")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [Column("ShortName", "简称", true, false, false, 160, "")]
        public string ShortName { get; set; }

        /// <summary>
        /// 物料流通码
        /// </summary>
        [Column("TrackCode", "流通码", false, false, false, 160, "")]
        public string TrackCode { get; set; }

        /// <summary>
        /// 基准单位
        /// </summary>
        [Column("UnitID", "单位", true, false, false, 160, "UnitName")]
        public int UnitID { get; set; }

        /// <summary>
        /// 物料组
        /// </summary>
        [Column("MaterialGroupID", "物料组", true, false, false, 160, "MaterialGroupName")]
        public int MaterialGroupID { get; set; }

        /// <summary>
        /// 默认存储单元
        /// </summary>
        [Column("LoctionID", "默认仓库", false, false, false, 160, "LocationDesc")]
        public int LoctionID { set; get; }

        /// <summary>
        /// 物料类型
        /// </summary>
        [Column("MatType", "物料类型", false, false, false, 160, "")]
        public string MatType { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [Column("Spec", "规格型号", false, false, false, 160, "")]
        public string Spec { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [Column("Model", "型号", false, false, false, 160, "")]
        public string Model { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [Column("Brand", "品牌", false, false, false, 160, "")]
        public string Brand { get; set; }

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
        [Column("UpdateBy", "更新人", true, false, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", true, false, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark", "备注", true, false, false, 160, "")]
        public string Remark { get; set; }

        /// <summary>
        /// 存储数量
        /// </summary>
        [Column("StorageCount", "存储数量", true, false, false, 160, "")]
        public int StorageCount { set; get; }

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
        /// 基本单位
        /// </summary>
        [ResultColumn]
        public string UnitName { get; set; }

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
        /// 物料组
        /// </summary>
        [ResultColumn]
        public string MaterialGroupName { get; set; }

        /// <summary>
        /// 默认存储单元
        /// </summary>
        [ResultColumn]
        public string LocationDesc { set; get; }

    }
}
