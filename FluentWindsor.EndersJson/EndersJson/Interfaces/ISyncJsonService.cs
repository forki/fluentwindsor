using System;
using System.Net.Http;

namespace FluentlyWindsor.EndersJson.Interfaces
{
    public interface ISyncJsonService : IDisposable
    {
        T Get<T>(string uri, object data = null);
        HttpResponseMessage Get(string uri, object data = null);
        string GetString(string uri, object data = null);
        T Post<T>(string uri, object data = null, bool dontSerialize = false);
        HttpResponseMessage Post(string uri, object data = null, bool dontSerialize = false);
        T Put<T>(string uri, object data = null, bool dontSerialize = false);
        HttpResponseMessage Put(string uri, object data = null, bool dontSerialize = false);
        T Delete<T>(string uri);
        HttpResponseMessage Delete(string uri);
    }
}