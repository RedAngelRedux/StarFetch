using Microsoft.JSInterop;
using System.Text.Json;

namespace StarFetch.Services;

public interface ILocalStorageService
{
    public Task SetItemAsync<T>(string key, T item);
    public Task<T?> GetItemAsync<T>(string key);
    public Task RemoveItemAsync(string key);
}
