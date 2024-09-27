namespace LruCacheSample.LRU;

public class NaiveLruCache<TKey, TValue> : ILruCache<TKey, TValue>
	where TKey : notnull
	where TValue : class {
	private sealed class CacheEntry<TEntryKey, TEntryValue> {
		public CacheEntry(TEntryKey key, TEntryValue value) {
			Key = key;
			Value = value;
		}

		public TEntryKey Key { get; }

		public TEntryValue Value { get; }
	}

	private readonly int capacity;
	private readonly LinkedList<CacheEntry<TKey, TValue>> cacheList = new ();

	public NaiveLruCache(int capacity) {
		if (capacity <= 0) {
			throw new ArgumentOutOfRangeException(nameof(capacity));
		}

		this.capacity = capacity;
	}

	public int Count => cacheList.Count;

	public bool TryGetValue(TKey key, out TValue? value) {
		var cacheEntry = cacheList.FirstOrDefault(x => x.Key.Equals(key));
		if (cacheEntry is not null) {
			value = cacheEntry.Value;
			cacheList.Remove(cacheEntry);
			cacheList.AddFirst(cacheEntry);
			return true;
		}

		value = null;
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
		cacheList.AddFirst(cacheEntry);

		return true;
	}

	public bool Remove(TKey key) {
		var cacheEntry = cacheList.FirstOrDefault(x => x.Key.Equals(key));
		if (cacheEntry is not null) {
			cacheList.Remove(cacheEntry);
			return true;
		}

		return false;
	}

	public bool ContainsKey(TKey key) => cacheList.Any(x => x.Key.Equals(key));

	public void Clear() {
		cacheList.Clear();
	}

	private void EvictLeastUsedOverCapacity() {
		while (cacheList.Count >= capacity) {
			var last = cacheList.Last;
			if (last is not null) {
				cacheList.RemoveLast();
			}
		}
	}
}