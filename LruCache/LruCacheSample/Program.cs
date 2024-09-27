// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using LruCacheSample;

BenchmarkRunner.Run<LruCacheBenchmark>();