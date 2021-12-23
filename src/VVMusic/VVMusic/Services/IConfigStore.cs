using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VVMusic.Models;

namespace VVMusic.Services
{
    public interface IConfigStore<T>
    {
        ServerInfo ServerInfo { get; set; }

        Task SaveConfigAsync(T config);

        Task<T> LoadConfigAsync();
    }
}
