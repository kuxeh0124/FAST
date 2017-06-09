using System;
using System.Configuration;
using System.IO;

namespace CoffeeBeanLibrary
{
    /// <summary>
    /// Read the config value in App.Config
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Update the value in App.config
        /// </summary>
        /// <param name="key">key string</param>
        /// <param name="value">value to be set</param>
        public static void UpdateConfig(string key, string value)
        {

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            
        }

        /// <summary>
        /// Get the value fron App.config
        /// </summary>
        /// <param name="key">key string</param>
        /// <returns>the value of key</returns>
        public static string GetKey(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
        
        /// <summary>
        /// Creates or appends a content on a text file
        /// </summary>
        /// <param name="filePath">Desired file to create / append</param>
        /// <param name="content">Content to be supplied</param>
        public static void WriteFileAppend(String filePath, String content)
        {
            try
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            catch
            {

            }
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    file.Write(content);
                }
            }
            catch { }
        }

        /// <summary>
        /// Creates or replaces a content on a text file
        /// </summary>
        /// <param name="filePath">Desired file to create / replace</param>
        /// <param name="content">Content to be supplied</param>
        public static void WriteFileReplace(String filePath, String content)
        {
            try
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            catch
            {

            }
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.Write(content);
                }
            }
            catch { }
        }
    }
}
