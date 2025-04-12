namespace pokeAPI.Services.Cache;

public interface ICache<in TKey, TValue, TArray>
{
    TValue GetData(TKey key);
    void SetData(TKey key, TValue value);
    
    TArray[] GetArrayData(int limit, int offset);
    
    void SetArrayData(TArray[] value, int limit, int offset, int bound);
    void Clear();
}