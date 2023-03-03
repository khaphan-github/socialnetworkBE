using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;

namespace SocialNetworkBE.Services.RedisCache {
    public class RedisCacheService {
        private readonly string Host = "localhost";
        private readonly int Port = 8080;
        private readonly RedisEndpoint redisEndpoint;

        public RedisCacheService() {
            redisEndpoint = new RedisEndpoint(Host, Port);
        }

        public void SetObjectToCache(string key, object objectValue) {
            var regisClient = new RedisClient(redisEndpoint);
            string jsonObjectData = JsonSerializer.Serialize<object>(objectValue);
            regisClient.SetValue(key, jsonObjectData);
        }
        public bool IsExistsKey(string key) {
            using (var redisClient = new RedisClient(redisEndpoint)) {
                if (redisClient.ContainsKey(key)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        // JsonSerializer.Deserialize<Object>(jsonString); To get object from cache
        public string GetJsonObjectFromCache(string key) {
            if (IsExistsKey(key)) {
                var regisClient = new RedisClient(redisEndpoint);
                return regisClient.GetValue(key);
            }
            return "";
        }

        public bool SetListObjectToCache<T>(string key, T value, TimeSpan timeout) {
            try {
                using (var redisClient = new RedisClient(redisEndpoint)) {
                    redisClient.As<T>().SetValue(key, value, timeout);
                }
                return true;
            } catch (Exception exx) {
                System.Diagnostics.Debug.WriteLine(exx);
                throw;
            }
        }
        public T GetList<T>(string key) {
            T result;

            using (var client = new RedisClient(redisEndpoint)) {
                var wrapper = client.As<T>();

                result = wrapper.GetValue(key);
            }

            return result;
        }
    }
}