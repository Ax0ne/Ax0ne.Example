using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Infrastructure
{

    public class JsonConfigObject
    {

    }
    /// <summary>
    /// 全局配置 （Json配置）
    /// </summary>
    public class GlobalConfig
    {
        private static DateTime? _fileLastWriteTime;
        private readonly static string _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "Example.json");
        private static JsonConfigObject _jsonConfigObject;
        /// <summary>
        /// 配置值
        /// </summary>
        public static JsonConfigObject Configuration
        {
            get
            {
                var fileLastWriteTime = File.GetLastWriteTime(_jsonFilePath);
                if (_fileLastWriteTime.HasValue && _jsonConfigObject != null)
                {
                    if (_fileLastWriteTime.Value == fileLastWriteTime) return _jsonConfigObject;
                }
                _fileLastWriteTime = fileLastWriteTime;
                var json = File.ReadAllText(_jsonFilePath);
                var configuration = JsonConvert.DeserializeObject<JsonConfiguration>(json);
                var configValue = configuration.IsTest ? configuration.Offline : configuration.Online;
                _jsonConfigObject = configValue;
                return configValue;
            }
        }
        /// <summary>
        /// Json 配置文件路径
        /// </summary>
        public static string JsonConfigFilePath { get { return _jsonFilePath; } }
    }

    internal class JsonConfiguration
    {
        /// <summary>
        /// 是否是测试配置
        /// </summary>
        public bool IsTest { get; set; }
        /// <summary>
        /// 线上配置（IsTest=false）
        /// </summary>
        public JsonConfigObject Online { get; set; }
        /// <summary>
        /// 线下配置（IsTest=true）
        /// </summary>
        public JsonConfigObject Offline { get; set; }
    }
}
