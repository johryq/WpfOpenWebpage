using System.Configuration;

namespace WpfOpenWebpage
{

    class ConfigureAppConfig
    {
        //静态构造,不能实例化
        static ConfigureAppConfig() { }

        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /**//// <summary>
            /// 获取AppSettings配置节中的Key值
            /// </summary>
            /// <param name="keyName">Key's name</param>
            /// <returns>Key's value</returns>
        public static string GetAppSettingsKeyValue(string keyName)
        {
            return ConfigurationManager.AppSettings.Get(keyName);
        }

        /// <summary>
        /// 增改皆可的方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAndUPAppSeting(string key, string value)
        {
            if (AppSettingsKeyExists(key))
            {
                UpdataAppSeting(key, value);
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Full);
            }
        }

        public static void DellAll()
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                ConfigurationManager.AppSettings.Remove(key);
            }
            RefreshAppSeting();
        }

        private static void UpdataAppSeting(string key, string value)
        {
            config.AppSettings.Settings[key].Value = value;
            config.Save();
        }

        /**//// <summary>
            /// 判断appSettings中是否有此项
            /// </summary>
        private static bool AppSettingsKeyExists(string strKey)
        {
            foreach (string str in config.AppSettings.Settings.AllKeys)
            {
                if (str == strKey)
                {
                    return true;
                }
            }
            return false;
        }

        public static void RefreshAppSeting()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }

        /**//// <summary>
            /// 保存appSettings中某key的value值
            /// </summary>
            /// <param name="strKey">key's name</param>
            /// <param name="newValue">value</param>
        //public static void AppSettingsSave(string strKey, string newValue)
        //{
        //    if (AppSettingsKeyExists(strKey))
        //    {
        //        config.AppSettings.Settings[strKey].Value = newValue;
        //        config.Save(ConfigurationSaveMode.Modified);
        //        ConfigurationManager.RefreshSection("appSettings");
        //    }
        //}
    }
}
