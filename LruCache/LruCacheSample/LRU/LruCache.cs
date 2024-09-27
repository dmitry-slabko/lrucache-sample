namespace LruCacheSample.LRU;

public class LruCache<TKey, TValue> : ILruCache<TKey, TValue>
	where TKey : notnull
	where TValue : class {
	private readonly struct CacheEntry<TEntryKey, TEntryValue> {
		public CacheEntry(TEntryKey key, TEntryValue value) {
			Key = key;
			Value = value;
		}

		public TEntryKey Key { get; }

		public TEntryValue Value { get; }
	}

	private readonly int capacity;
	private readonly LinkedList<TKey> cacheList = new ();
	private readonly Dictionary<TKey, CacheEntry<TKey, TValue>> cacheDict;

	public LruCache(int capacity) {
		if (capacity <= 0) {
			throw new ArgumentOutOfRangeException(nameof(capacity));
		}

		this.capacity = capacity;
		cacheDict = new (capacity);
	}

	public int Count => cacheDict.Count;

	public bool TryGetValue(TKey key, out TValue? value) {
		if (cacheDict.TryGetValue(key, out var cacheEntry)) {
			value = cacheEntry.Value;
			cacheList.Remove(cacheEntry.Key);
			cacheList.AddFirst(cacheEntry.Key);
			return true;
		}

		value = default;
		return false;
	}

	public bool Add(TKey key, TValue value) {
		if (TryGetValue(key, out var existing)) {
			if (value == existing) {
				return false;
			}

			Remove(key);
		} else {
			EvictLeastUsedOverCapacity();
		}

		var cacheEntry = new CacheEntry<TKey, TValue>(key, value);
		cacheList.AddFirst(cacheEntry.Key);
		cacheDict.Add(key, cacheEntry);

		return true;
	}

	public bool Remove(TKey key) {
		if (cacheDict.Remove(key)) {
			cacheList.Remove(key);
			return true;
		}

		return false;
	}

	public bool ContainsKey(TKey key) => cacheDict.ContainsKey(key);

	public void Clear() {
		cacheDict.Clear();
		cacheList.Clear();
	}

	private void EvictLeastUsedOverCapacity() {
		while (cacheDict.Count >= capacity) {
			var last = cacheList.Last;
			if (last is not null) {
				cacheList.RemoveLast();
				cacheDict.Remove(last.Value);
			}
		}
	}
}