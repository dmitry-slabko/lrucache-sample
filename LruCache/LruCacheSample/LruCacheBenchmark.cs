using BenchmarkDotNet.Attributes;
using LruCacheSample.LRU;

namespace LruCacheSample;

/*
| Method             | Capacity | Mean            | Ratio | Gen0        | Gen1      | Allocated     | Alloc Ratio |
|------------------- |--------- |----------------:|------:|------------:|----------:|--------------:|------------:|
| NaiveLruCache      | 1000     |    23,755.54 us | 1.006 |   7718.7500 |  562.5000 |   47296.89 KB |       1.000 |
| LruCache           | 1000     |        55.98 us | 0.002 |     15.2588 |    2.1362 |      93.75 KB |       0.002 |
| LruCacheOverString | 1000     |       217.99 us | 0.009 |     23.9258 |    4.6387 |     146.88 KB |       0.003 |
|                    |          |                 |       |             |           |               |             |
| NaiveLruCache      | 10000    | 2,811,296.40 us | 1.017 | 765000.0000 | 1000.0000 | 4691719.14 KB |       1.000 |
| LruCache           | 10000    |       622.96 us | 0.000 |    152.3438 |  151.3672 |      937.5 KB |       0.000 |
| LruCacheOverString | 10000    |     1,930.41 us | 0.001 |    251.9531 |  250.0000 |    1553.13 KB |       0.000 |
 */

[MemoryDiagnoser]
[ShortRunJob]
public class LruCacheBenchmark {
	private const string Value = "ABCDEFGHIJKLMNOPQRSTUV";
	private LruCache<int, string> lruCache = null!;
	private NaiveLruCache<int, string> naiveLruCache = null!;
	private LruCache<string, string> lruCacheOverString = null!;

	[Params(1000, 10000)]
	public int Capacity { get; set; }

	[GlobalSetup]
	public void Setup() {
		lruCache = new LruCache<int, string>(Capacity);
		naiveLruCache = new NaiveLruCache<int, string>(Capacity);
		lruCacheOverString = new LruCache<string, string>(Capacity);
	}

	[Benchmark(Baseline = true)]
	public ILruCache<int, string> NaiveLruCache() {
		for (int i = 0; i < Capacity * 2; i++) {
			naiveLruCache.Add(i, Value);
		}

		return naiveLruCache;
	}

	[Benchmark]
	public ILruCache<int, string> LruCache() {
		for (int i = 0; i < Capacity * 2; i++) {
			lruCache.Add(i, Value);
		}

		return lruCache;
	}

	[Benchmark]
	public ILruCache<string, string> LruCacheOverString() {
		for (int i = 0; i < Capacity * 2; i++) {
			lruCacheOverString.Add(i.ToString(), Value);
		}

		return lruCacheOverString;
	}
}