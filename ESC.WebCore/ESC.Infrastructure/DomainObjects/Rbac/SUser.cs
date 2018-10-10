using Dapper.Extensions;
using System;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 用户 +
    /// </summary>
    public class SUser : PocoObject
    {
        public SUser()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
            Birthday = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 编码
        /// </summary>
       [Column("UserCode", "用户编码", true, true)]
        public string UserCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
       [Column("UserName", "用户名称", true, true)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
       [Column("Pwd", "密码", false)]
        public string Pwd { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
       [Column("SuperUser", "是否管理员", true, true, DisplayColumn = "SuperUserName")]
        public int SuperUser { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
       [Column("EMail", "邮箱", true, false)]
        public string EMail { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
       [Column("OrgID", "部门", true, false,DisplayColumn ="OrgName")]
        public int OrgID { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
       [Column("ImageUrl", "头像",false)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
       [Column("Birthday", "生日", false)]
        public DateTime? Birthday { get; set; }

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
        /// 领导
        /// </summary>
       [Column("LeaderID", "领导", false)]
        public int LeaderID { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
       [Column("Mobile", "电话", false)]
        public string Mobile { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
       [Column("Phone", "手机号码", false)]
        public string Phone { get; set; }       
      
        /// <summary>
        /// 可用状态
        /// </summary>
       [Column("InUse", "可用状态", false)]
        public int InUse { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
       [Column("Remark", "备注", true, false)]
        public string Remark { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
       [Column("Address", "地址", false)]
        public string Address { get; set; }

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

        /// <summary>
        /// 部门
        /// </summary>
        [ResultColumn("OrgName", "部门", false)]
        public string OrgName { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        [ResultColumn("SuperUserName", "是否管理员", false)]
        public string SuperUserName { get; set; }

        /// <summary>
        /// 领导
        /// </summary>
        [ResultColumn("LeaderName", "领导", false)]
        public string LeaderName { get; set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        [ResultColumn("InUseName", "可用状态", false)]
        public string InUseName { set; get; }

    }
}
