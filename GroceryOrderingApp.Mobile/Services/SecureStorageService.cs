using System.Security.Cryptography;
using System.Text;

namespace GroceryOrderingApp.Mobile.Services
{
    public interface ISecureStorageService
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
        Task RemoveAsync(string key);
    }

    public class SecureStorageService : ISecureStorageService
    {
        public async Task<string?> GetAsync(string key)
        {
            try
            {
                return await SecureStorage.GetAsync(key);
            }
            catch
            {
                return null;
            }
        }

        public async Task SetAsync(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch { }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                SecureStorage.Remove(key);
                await Task.CompletedTask;
            }
            catch { }
        }
    }
}
