using System;
using System.Collections.Specialized;
using System.Configuration;

namespace ESC.Core
{
    /// <summary>
    /// 在更新配置文件的时候,总是存在更新丢失的现象,所以自己手动修改,摒弃VS自带功能
    /// </summary>
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        public ConfigurationManagerWrapper()
        {

        }

        /// <summary>
        /// 配置节点
        /// </summary>
        public NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            ConnectionStringSettings ccSetings = ConfigurationManager.ConnectionStrings[name];
            if (ccSetings == null)
                return string.Empty;
            else
                return ccSetings.ConnectionString;
        }

        /// <summary>
        /// 获取提供程序名的属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetProviderName(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ProviderName;
        }

        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public T GetSection<T>(string sectionName)
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }

        /// <summary>
        /// 添加链接字符串
        /// 如果存在相同键,则忽略
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddConnectionString(string key, string value)
        {
            string result = GetConnectionString(key);
            if (string.IsNullOrEmpty(result))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConnectionStringSettings csSetting = new ConnectionStringSettings(key, value);
                config.ConnectionStrings.ConnectionStrings.Add(csSetting);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
        }

        /// <summary>
        /// 添加配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddAppSetting(string key, string value)
        {
            string result = this.AppSettings[key];
            if (string.IsNullOrEmpty(result))
            {
                Configuration config =
                 ConfigurationManager.OpenExeConfiguration(
                 ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        /// 更新配置节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateSetting(string key, string value)
        {
            string result = this.AppSettings[key];
            if (!string.IsNullOrEmpty(result))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        /// 更新连接字符串节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateConnectionString(string key, string value)
        {
            string result = GetConnectionString(key);
            if (!string.IsNullOrEmpty(result))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
        }

        /// <summary>
        /// 删除配置节点
        /// </summary>
        /// <param name="key"></param>
        public void RemoveSetting(string key)
        {
            string result = this.AppSettings[key];
            if (!string.IsNullOrEmpty(result))
            {
                Configuration config =
                 ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        /// 删除连接节点
        /// </summary>
        /// <param name="key"></param>
        public void RemoveConnectionString(string key)
        {
            string result = GetConnectionString(key);
            if (!string.IsNullOrEmpty(result))
            {
                Configuration config =
                 ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
        }
    }
}
