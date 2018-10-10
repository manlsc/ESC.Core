using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 单号生成规则
    /// </summary>
    public class SNumberRule : PocoObject
    {
        public SNumberRule()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        [Column("BusinessType", "业务类型", true, true)]
        public string BusinessType { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        [Column("Prefix", "前缀", true, true)]
        public string Prefix { get; set; }

        /// <summary>
        /// 序列名称
        /// </summary>
        [Column("SeqName", "序列名称", false, true)]
        public string SeqName { get; set; }

        /// <summary>
        /// 单号长度
        /// </summary>
        [Column("RuleLength", "单号长度", true, true)]
        public int RuleLength { get; set; }

        /// <summary>
        /// 起始序号
        /// </summary>
        [Column("StartSeq", "起始序号", true, true)]
        public int StartSeq { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark", "备注", true, true)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, false, true, 180, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, false, true, 180)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", true, false, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", true, false, true, 180)]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ResultColumn("CreateByUserName", "创建人", false)]
        public string CreateByUserName { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [ResultColumn("UpdateByUserName", "更新人", false)]
        public string UpdateByUserName { get; set; }

    }
}
