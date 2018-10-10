using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ESC.Core;
using System.Text;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public partial class DatabaseContext
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        string ConnectionString = "";
        //非单例
        public IDbConnection Connection;
        public IDbTransaction Transcation;

        /// <summary>
        /// 构造函数 
        /// 默认链接字符串 SqlServerConnStr
        /// </summary>
        public DatabaseContext()
            : this("ConnectionStrings:Default")
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStringName">链接字符串</param>
        public DatabaseContext(string connectionStringName)
        {
            this.ConnectionString = AppSetting.GetConfig(connectionStringName);

            Connection = GetConnection();
        }

        partial void CommonConstruct();

        /// <summary>
        /// 获取Connection
        /// </summary>
        /// <param name="mars"></param>
        /// <returns></returns>
        protected SqlConnection GetConnection(bool mars = false)
        {
            var cs = ConnectionString;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs)
                {
                    MultipleActiveResultSets = true
                };
                cs = scsb.ConnectionString;
            }
            return new SqlConnection(cs);
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            if (Transcation == null)
            {
                if (this.Connection.State == ConnectionState.Closed)
                {
                    this.Connection.Open();
                }
                Transcation = this.Connection.BeginTransaction();
            }
        }

        /// <summary>
        /// 完成事务
        /// </summary>
        public void CompleteTransaction()
        {
            if (Transcation != null)
            {
                Transcation.Commit();
            }
            if (this.Connection.State == ConnectionState.Open)
            {
                this.Connection.Close();
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void AbortTransaction()
        {
            if (Transcation != null)
            {
                Transcation.Rollback();
            }
            if (this.Connection.State == ConnectionState.Open)
            {
                this.Connection.Close();
            }
        }
    }
}
