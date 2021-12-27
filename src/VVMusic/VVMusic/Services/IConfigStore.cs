using VVMusic.Models;
using System.Threading.Tasks;

namespace VVMusic.Services
{
    /// <summary>
    /// 配置文件Store
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConfigStore<T>
    {
        ServerInfo ServerInfo { get; set; }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        Task SaveConfigAsync(T config);

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        Task<T> LoadConfigAsync();
    }
}
