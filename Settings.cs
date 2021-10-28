using System;
using System.Configuration;

namespace Regularity_Rally
{
    class Settings
    {
        private static readonly Settings instance = new Settings();

        #region ConfigBase

        private Configuration m_Cnf;

        #endregion

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Settings()
        {
            Load();
        }

        private Settings()
        {
        }

        public static Settings Instance
        {
            get
            {
                return instance;
            }
        }

        public static void Load()
        {
            instance.m_Cnf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static string GetValue(string key, string default_value)
        {
            foreach (string fkey in instance.m_Cnf.AppSettings.Settings.AllKeys)
            {
                if (fkey.Equals(key))
                {
                    return instance.m_Cnf.AppSettings.Settings[key].Value;
                }
            }
            return default_value;
        }

        public static byte[] GetValue(string key)
        {
            foreach (string fkey in instance.m_Cnf.AppSettings.Settings.AllKeys)
            {
                if (fkey.Equals(key))
                {
                    return System.Convert.FromBase64String(instance.m_Cnf.AppSettings.Settings[key].Value);
                }
            }
            return null;
        }

        public static void SetValue(string key, byte[] value)
        {
            try
            {
                var settings = instance.m_Cnf.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, System.Convert.ToBase64String(value));
                }
                else
                {
                    settings[key].Value = System.Convert.ToBase64String(value);
                }
                instance.m_Cnf.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(instance.m_Cnf.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {

            }
        }

        public static void SetValue(string key, string value)
        {
            try
            {
                var settings = instance.m_Cnf.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                instance.m_Cnf.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(instance.m_Cnf.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {

            }
        }
    }
}
