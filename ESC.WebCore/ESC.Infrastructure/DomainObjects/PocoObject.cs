using System;
using System.Text;
using System.Data;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 基础类
    /// </summary>
    [PrimaryKey("ID")]
    public class PocoObject
    {
        public PocoObject()
        {
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID", "主键", false, false)]
        public int ID { get; set; }

        /// <summary>
        /// 用于标记实体的操作
        /// add\delete\update
        /// </summary>
        [Ignore]
        public string CURD { set; get; }
    }
}