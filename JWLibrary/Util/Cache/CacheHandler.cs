using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using eXtensionSharp;
using NetFabric.Hyperlinq;

namespace JWLibrary.Util.Cache {
    public class CacheResolver {
        private static Lazy<CacheResolver> _instance = 
            new Lazy<CacheResolver>(() => new CacheResolver());

        public static CacheResolver Instance {
            get
            {
                return _instance.Value;
            }
        }

        private CacheHandler _cacheHandlerObj = new();
        
        private CacheResolver() {
            
        }

        public TEntity Resolve<TResolver, TEntity>() where TResolver : CacheResolverBase<TEntity>, new() {
            TResolver resolver = new TResolver();
            
            var key = resolver.InitKey();
            var value = _cacheHandlerObj.Get<string>(key); 
            
            if (value.xIsEmpty()) {
                var valueObj = resolver.GetValue();
                if (_cacheHandlerObj.Add(key, valueObj.xObjectToJson())) {
                    return valueObj;
                }
            }

            var isReset = (DateTime.Now - value.CachedDateTime).TotalSeconds > resolver.ResetInterval();
            if (isReset) {
                _cacheHandlerObj.Delete(key);
            }

            return value.ValueObject.xJsonToObject<TEntity>();
        }

        public int Count() {
            return _cacheHandlerObj.Count();
        }
    } 

    public abstract class CacheResolverBase<T> {
        public abstract string InitKey();
        public abstract T GetValue();
        public abstract int ResetInterval();
    }
}