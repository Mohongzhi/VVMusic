using System.IO;
using RestSharp;
using VVMusic.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VVMusic.Services
{
    public interface IServerStore
    {
        IConfigStore<ServerInfo> ConfigStore { get; set; }

        bool IsConnected { get; set; }

        RestClient client { get; set; }

        Task<List<LinkItem>> GetLinkItemsAsync(string href = "");

        Task<Stream> DownloadMusicAsync(string href);

        Task<Stream> DownloadMusicLrcAsync(string href);

        Task<bool> TryConnectAsync(string serverAddress, string userName, string password);
    }
}
