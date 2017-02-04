using System.Net.Http;
using FluentlyWindsor.EndersJson.Interfaces;
using Nito.AsyncEx;

namespace FluentlyWindsor.EndersJson
{
    public class SyncJsonService : JsonService, ISyncJsonService
    {
        public T Get<T>(string uri, object data = null)
        {
            return AsyncContext.Run(() => this.GetAsync<T>(uri, data));
        }

        public HttpResponseMessage Get(string uri, object data = null)
        {
            return AsyncContext.Run(() => this.GetAsync(uri, data));
        }

        public string GetString(string uri, object data = null)
        {
            return AsyncContext.Run(() => this.GetStringAsync(uri, data));
        }

        public T Post<T>(string uri, object data = null, bool dontSerialize = false)
        {
            return AsyncContext.Run(() => this.PostAsync<T>(uri, data, dontSerialize));
        }

        public HttpResponseMessage Post(string uri, object data = null, bool dontSerialize = false)
        {
            return AsyncContext.Run(() => this.PostAsync(uri, data, dontSerialize));
        }

        public T Put<T>(string uri, object data = null, bool dontSerialize = false)
        {
            return AsyncContext.Run(() => this.PutAsync<T>(uri, data, dontSerialize));
        }
        public HttpResponseMessage Put(string uri, object data = null, bool dontSerialize = false)
        {
            return AsyncContext.Run(() => this.PutAsync(uri, data, dontSerialize));
        }

        public T Delete<T>(string uri)
        {
            return AsyncContext.Run(() => this.DeleteAsync<T>(uri));
        }

        public HttpResponseMessage Delete(string uri)
        {
            return AsyncContext.Run(() => this.DeleteAsync(uri));
        }
    }
}