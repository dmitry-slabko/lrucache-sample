namespace LruCacheSample.LRU;

public interface ILruCache<TKey, TValue> 
	where TKey : notnull 
	where TValue : class {
	int Count { get; }

	bool TryGetValue(TKey key, out TValue? value);

	//TValue? GetValue(TKey key);

	bool Add(TKey key, TValue value);

	bool Remove(TKey key);

	bool ContainsKey(TKey key);

	void Clear();
}