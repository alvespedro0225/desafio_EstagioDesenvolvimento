using System.Collections.Concurrent;

namespace pokeAPI.Services.Cache;

public class DictionaryCache<TKey, TValue>(IDictionary<TKey, TValue> cache)
    : ICache<TKey, TValue>
    where TKey : notnull
{
    public TValue GetData(TKey key)
    {
        if (cache.TryGetValue(key, out TValue? value))
            return value;
        
        throw new KeyNotFoundException($"The key {key} was not present in the cache.");
    }

    public void SetData(TKey key, TValue value)
    {
        if (cache.TryAdd(key, value))
            return;
        
        throw new ArgumentException($"The key {key} was already present in the cache.");
    }

    public void Clear()
    {
        cache.Clear();
    }
}