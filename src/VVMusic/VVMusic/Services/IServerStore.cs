using System.IO;
using RestSharp;
using VVMusic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VVMusic.Services
{
    /// <summary>
    /// 与IIS WebDav通信Store
    /// </summary>
    public interface IServerStore
    {
        /// <summary>
        /// 配置Store
        /// </summary>
        IConfigStore<ServerInfo> ConfigStore { get; set; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        bool IsConnected { get; set; }

        RestClient client { get; set; }

        /// <summary>
        /// 获取链接选项
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        Task<List<LinkItem>> GetLinkItemsAsync(string href = "");

        /// <summary>
        /// 下载音乐
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        Task<Stream> DownloadMusicAsync(string href);

        /// <summary>
        /// 下载歌词
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        Task<Stream> DownloadMusicLrcAsync(string href);

        /// <summary>
        /// 尝试连接
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> TryConnectAsync(string serverAddress, string userName, string password);
    }
}
