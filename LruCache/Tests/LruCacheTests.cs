using FluentAssertions;
using LruCacheSample.LRU;

namespace Tests;

public class LruCacheTests {
	private static ILruCache<TKey, TValue> CreateCache<TKey, TValue>(int capacity)
		where TKey : notnull
		where TValue : class {
		return new LruCache<TKey, TValue>(capacity);
		// return new NaiveLruCache<TKey, TValue>(capacity);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(100)]
	[InlineData(1000)]
	[InlineData(100_000)]
	public void NewInstance_DoesNotThrow(int capacity) {
		// Arrange
		var action = () => CreateCache<int, string>(capacity);

		// Act & Assert
		action.Should().NotThrow();
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	public void NewInstance_Throws_For_InvalidCapacity(int capacity) {
		// Arrange
		var action = () => CreateCache<int, string>(capacity);

		// Act & Assert
		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void AddNewItem() {
		// Arrange
		var lruCache = CreateCache<int, string>(10);

		// Act
		bool added = lruCache.Add(1, "test");

		// Assert
		added.Should().BeTrue();
		lruCache.Count.Should().Be(1);
	}

	[Fact]
	public void AddSameItem() {
		// Arrange
		var lruCache = CreateCache<int, string>(10);
		lruCache.Add(1, "test");

		// Act
		bool added = lruCache.Add(1, "test");

		// Assert
		added.Should().BeFalse();
		lruCache.Count.Should().Be(1);
	}

	[Fact]
	public void AddSameKeyDifferentValue() {
		// Arrange
		var lruCache = CreateCache<int, string>(10);
		lruCache.Add(1, "test");

		// Act
		bool added = lruCache.Add(1, "test2");

		// Assert
		added.Should().BeTrue();
		lruCache.Count.Should().Be(1);
	}

	[Theory]
	[InlineData(1, true)]
	[InlineData(2, false)]
	public void GetItem(int key, bool expectedResult) {
		// Arrange
		var lruCache = CreateCache<int, string>(10);
		lruCache.Add(1, "test");

		// Act
		bool result = lruCache.TryGetValue(key, out _);

		// Assert
		result.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData(1, 0, true)]
	[InlineData(2, 1, false)]
	public void RemoveItem(int key, int expectedCount, bool expectedRemoved) {
		// Arrange
		var lruCache = CreateCache<int, string>(10);
		lruCache.Add(1, "test");

		// Act
		bool removed = lruCache.Remove(key);

		// Assert
		removed.Should().Be(expectedRemoved);
		lruCache.Count.Should().Be(expectedCount);
	}

	[Theory]
	[InlineData(1, true)]
	[InlineData(11, false)]
	public void ContainsKey(int key, bool expectedResult) {
		// Arrange
		var lruCache = CreateCache<int, string>(10);
		lruCache.Add(1, "test");

		// Act
		bool result = lruCache.ContainsKey(key);

		// Assert
		result.Should().Be(expectedResult);
	}

	[Fact]
	public void AddItemsOverCapacity() {
		// Arrange
		const int capacity = 10;
		var lruCache = CreateCache<int, string>(capacity);

		// Act
		for (int i = 0; i < capacity * 2; i++) {
			lruCache.Add(i, i.ToString());
		}

		// Assert
		lruCache.Count.Should().Be(capacity);
	}

	[Fact]
	public void Clear() {
		// Arrange
		var lruCache = CreateCache<int, string>(10);
		lruCache.Add(1, "test");

		// Act
		lruCache.Clear();

		// Assert
		lruCache.Count.Should().Be(0);
	}
}