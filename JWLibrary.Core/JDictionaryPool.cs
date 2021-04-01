﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace JWLibrary.Core {
    public class JDictionaryPool<TKey, TValue> {
        private readonly ConcurrentQueue<TKey> _keyQueue;
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> _pool;
        private readonly byte _poolSize;

        public JDictionaryPool(byte maxPoolSize) {
            if (maxPoolSize <= 0)
                maxPoolSize = (byte) Math.Min(Environment.ProcessorCount, byte.MaxValue);

            _poolSize = maxPoolSize;
            _keyQueue = new ConcurrentQueue<TKey>();
            _pool = new ConcurrentDictionary<TKey, ConcurrentQueue<TValue>>(Environment.ProcessorCount * 2, _poolSize);
        }

        public bool Add(TKey key, TValue value) {
            if (key.jIsNull())
                return false;

            if (!_pool.ContainsKey(key) && _pool.TryAdd(key, new ConcurrentQueue<TValue>())) {
                _keyQueue.Enqueue(key);

                while (_pool.Count > _poolSize) {
                    TKey localKey;
                    if (_keyQueue.TryDequeue(out localKey)) Remove(localKey);
                }
            }

            ConcurrentQueue<TValue> q;
            if (_pool.TryGetValue(key, out q)) {
                q.Enqueue(value);
                while (q.Count > _poolSize) {
                    TValue localValue;
                    q.TryDequeue(out localValue);
                }
            }
            else {
                return false;
            }

            return true;
        }

        public bool Remove(TKey key) {
            if (key.jIsNull())
                return false;

            ConcurrentQueue<TValue> value;
            return _pool.TryRemove(key, out value);
        }

        public int Count(TKey key) {
            if (key.jIsNull())
                return 0;

            if (!_pool.ContainsKey(key))
                _pool.TryAdd(key, new ConcurrentQueue<TValue>());

            ConcurrentQueue<TValue> q;
            if (_pool.TryGetValue(key, out q)) return q.Count;
            return 0;
        }

        public TValue GetValue(TKey key, Func<TValue> creator = null) {
            if (key.jIsNull())
                return default;

            if (!_pool.ContainsKey(key))
                _pool.TryAdd(key, new ConcurrentQueue<TValue>());

            ConcurrentQueue<TValue> q;
            if (_pool.TryGetValue(key, out q)) {
                TValue v;
                if (q.TryDequeue(out v))
                    return v;
                if (creator.jIsNotNull())
                    return creator();
            }

            return default;
        }

        public bool Release(TKey key, TValue value) {
            return Add(key, value); //just adds it back to key's queue
        }
    }

    public static class JDictionaryUtil {
        /// <summary>
        /// dictionary concat.
        /// if same key exists, throw error. 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, T> jConcat<T>(this IDictionary<string, T> first, IDictionary<string, T> second) {
            return first.Concat(second)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// dictionary concat and update.
        /// if same key exists, update from second value.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, T> jConcatUpdate<T>(this IDictionary<string, T> first, IDictionary<string, T> second) {
            var result = new Dictionary<string, T>();
            first.jForEach(firstPair => {
                result.Add(firstPair.Key, firstPair.Value);
            });
            
            second.jForEach(secondPair => {
                var exists = result.ContainsKey(secondPair.Key);
                if (exists) {
                    if (result[secondPair.Key].jIsEmpty()) {
                        result[secondPair.Key] = secondPair.Value;
                    }
                }
                else {
                    result.Add(secondPair.Key, secondPair.Value);
                }
            });

            return result;
        }
    }
}