using System;
using System.Text;
using VVMusic.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace VVMusic.Services
{
    /// <summary>
    /// 服务端配置
    /// </summary>
    public class ServerConfigStore : IConfigStore<ServerInfo>
    {
        public ServerInfo ServerInfo { get; set; }

        public ServerConfigStore()
        {
            ServerInfo = LoadConfigAsync().Result;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<ServerInfo> LoadConfigAsync()
        {
            var config_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "server.config");
            if (File.Exists(config_path))
            {
                return JsonConvert.DeserializeObject<ServerInfo>(File.ReadAllText(config_path));
            }
            return null;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task SaveConfigAsync(ServerInfo config)
        {
            var config_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "server.config");
            var fileStream = new FileStream(config_path, FileMode.OpenOrCreate);
            using(var writer = new StreamWriter(fileStream))
            {
                writer.Write(JsonConvert.SerializeObject(config));
            }
        }
    }
}
