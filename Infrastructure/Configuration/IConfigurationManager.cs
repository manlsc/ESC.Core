using System;
using System.Collections.Specialized;

namespace ESC.Core
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings
        {
            get;
        }

        /// <summary>
        /// 链接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetConnectionString(string name);

        /// <summary>
        /// 链接数据库的Provider
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetProviderName(string name);

        /// <summary>
        /// 获取节点名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        T GetSection<T>(string sectionName);

        /// <summary>
        /// 添加连接节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddConnectionString(string key, string value);

        /// <summary>
        /// 添加配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddAppSetting(string key, string value);

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void UpdateSetting(string key, string value);

        /// <summary>
        /// 更新连接节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void UpdateConnectionString(string key, string value);

        /// <summary>
        /// 删除配置节点
        /// </summary>
        /// <param name="key"></param>
        void RemoveSetting(string key);

        /// <summary>
        /// 删除连接节点
        /// </summary>
        /// <param name="key"></param>
        void RemoveConnectionString(string key);

    }
}
