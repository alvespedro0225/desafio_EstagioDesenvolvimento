namespace pokeAPI.Services.Cache;

public interface ICache<in TKey, TValue>
{
    TValue? GetData(TKey key);
    void SetData(TKey key, TValue value);
    void Clear();
}