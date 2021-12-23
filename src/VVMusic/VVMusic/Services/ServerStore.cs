using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using VVMusic.Models;
using Xamarin.Forms;

namespace VVMusic.Services
{
    public class ServerStore : IServerStore
    {

        public bool IsConnected { get; set; } = false;

        public RestClient client { get; set; }

        public IConfigStore<ServerInfo> ConfigStore { get; set; }

        public ServerInfo ServerInfo { get; set; }

        public ServerStore()
        {
            ConfigStore = DependencyService.Get<IConfigStore<ServerInfo>>();
            ServerInfo = ConfigStore.ServerInfo;
            if (ServerInfo != null)
                IsConnected = TryConnectAsync(ConfigStore.ServerInfo.ServerAddress, ConfigStore.ServerInfo.UserName, ConfigStore.ServerInfo.Password).Result;
        }

        public async Task<Stream> DownloadMusicAsync(string href)
        {
            if (!IsConnected || ServerInfo == null)
                return null;

            client.Authenticator = new HttpBasicAuthenticator(ServerInfo.UserName, ServerInfo.Password);
            RestRequest request = new RestRequest("/" + ServerInfo.MusicFolder + "/" + href);
            request.AddHeader("Accept", "application/x-flac,audio/mpeg");
            var data = client.DownloadData(request);
            var memoryStream = BytesToStream(data);
            return memoryStream;
        }

        public async Task<Stream> DownloadMusicLrcAsync(string href)
        {
            if (!IsConnected || ServerInfo == null)
                return null;

            client.Authenticator = new HttpBasicAuthenticator(ServerInfo.UserName, ServerInfo.Password);
            RestRequest request = new RestRequest("/" + ServerInfo.MusicFolder + "/" + href);
            request.AddHeader("Accept", "text/plain");
            var res = client.DownloadData(request);
            return BytesToStream(res);
        }

        public async Task<List<LinkItem>> GetLinkItemsAsync(string href = "")
        {
            if (!IsConnected || ServerInfo == null)
                return null;

            List<LinkItem> networkItems = new List<LinkItem>();

            client.Authenticator = new HttpBasicAuthenticator(ServerInfo.UserName, ServerInfo.Password);
            RestRequest request = new RestRequest(href);
            var res = client.Execute(request, Method.GET);
            if (res.IsSuccessful)
            {
                var content = res.Content;
                var matchers = Regex.Matches(content, @"<A[\s\S]+?</A>");
                foreach (Match matcher in matchers)
                {
                    var inner_href = Regex.Match(matcher.Value, @"""[\s\S]+?""").Value.TrimStart('"').TrimEnd('"');
                    var fileName = Regex.Match(matcher.Value, @">[\s\S]+?<").Value.TrimStart('>').TrimEnd('<');
                    bool isFolder = false;
                    if (inner_href[inner_href.Length - 1] == '/')
                    {
                        isFolder = true;//斜杠结尾则是文件夹
                    }
                    networkItems.Add(new LinkItem()
                    {
                        Name = fileName,
                        Href = inner_href,
                        IsFolder = isFolder,
                    });
                }
            }

            return networkItems;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 尝试连接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TryConnectAsync(string serverAddress, string userName, string password)
        {
            try
            {
                client = new RestClient(serverAddress);
                client.Authenticator = new HttpBasicAuthenticator(userName, password);
                RestRequest request = new RestRequest();
                var res = client.Execute(request);
                IsConnected = res.IsSuccessful;
            }
            catch
            {
                IsConnected = false;
            }
            
            return IsConnected;
        }
    }
}
