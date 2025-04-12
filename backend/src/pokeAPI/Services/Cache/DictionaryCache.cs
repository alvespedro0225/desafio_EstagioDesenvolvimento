namespace pokeAPI.Services.Cache;

/// <summary>
/// In memory dictionary to cache API calls. Not using Redis and expiration as the data is static and not very large
/// </summary>
/// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
/// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
/// <typeparam name="TArray"> The type of the values in the array </typeparam>
public class DictionaryCache<TKey, TValue, TArray> : ICache<TKey, TValue, TArray>
    where TKey : notnull
{
    public DictionaryCache(IDictionary<TKey, TValue> cache)
    {
        _arrayCache.Initialize();
        _cache = cache;
    }

    private int? _bound;
    
    private readonly IDictionary<TKey, TValue> _cache;
    // Didn't find a way to inject an array with defined size
    private readonly TArray?[] _arrayCache = new TArray?[1500];
    public TValue GetData(TKey key)
    {
        if (_cache.TryGetValue(key, out TValue? value))
            return value;
        
        throw new KeyNotFoundException($"The key {key} was not present in the cache.");
    }

    public void SetData(TKey key, TValue value)
    {
        if (_cache.TryAdd(key, value))
            return;
        
        throw new ArgumentException($"The key {key} was already present in the cache.");
    }

    public TArray[] GetArrayData(int limit, int offset)
    {
        if (_bound is null) throw new KeyNotFoundException("The cache is null.");
        var data = new List<TArray>();
        for (var count = 0; count < limit && offset + count < _bound; count++)
        {
            if (_arrayCache[offset + count] is null) 
                throw new KeyNotFoundException($"The key {offset + count} was not present in the cache.");
            data.Add(_arrayCache[offset + count]!);
        }
        return data.ToArray();
    }

    public void SetArrayData(TArray[] value, int limit, int offset, int bound)
    {
        if (_bound is null) _bound = bound;
        for (var count = 0; count < limit && offset + count < bound; count++)
        {
            _arrayCache[offset + count] = value[count];
        }
    }

    public void Clear()
    {
        _cache.Clear();
    }
}