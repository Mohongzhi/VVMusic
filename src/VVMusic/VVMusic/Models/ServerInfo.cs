using System;
using System.Collections.Generic;
using System.Text;

namespace VVMusic.Models
{
    /// <summary>
    /// 连接服务端配置文件
    /// </summary>
    public class ServerInfo
    {
        public string ServerAddress { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string MusicFolder { get; set; }
    }
}
