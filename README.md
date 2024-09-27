This solution contains a sample implementation of LRU cache (Least Recently Used), as well as its benchmarks and unit tests.

Two implementations are given - NaiveLruCache and LruCache. The naive implementations uses only a linked list and it is inefficient, though works according to
the contract. LruCache implementation is more in line with how LRU cache should be implemented, as it provides a linked list to track cache items usage order and
a dictionary (hashtable) to store actual item data for fast access and manipulation.
