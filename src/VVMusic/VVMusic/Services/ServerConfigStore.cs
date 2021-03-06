using System;
using System.IO;
using VVMusic.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
            try
            {
                var config_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "server.config");
                if (File.Exists(config_path))
                {
                    var fileInfo = File.ReadAllText(config_path);
                    ServerInfo = JsonConvert.DeserializeObject<ServerInfo>(fileInfo);
                    return ServerInfo;
                }
            }
            catch(Exception ex)
            {
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
            try
            {
                var config_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "server.config");
                File.Delete(config_path);
                var fileStream = new FileStream(config_path, FileMode.OpenOrCreate);
                using (var writer = new StreamWriter(fileStream))
                {
                    var fs = JsonConvert.SerializeObject(config);
                    writer.Write(fs);
                }
                ServerInfo = config;
            }
            catch
            {

            }            
        }
    }
}
