using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentlyWindsor.EndersJson.Extensions;
using FluentlyWindsor.EndersJson.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FluentlyWindsor.EndersJson
{
    public class JsonService : IJsonService
    {
        private static readonly ConcurrentDictionary<string, string> Headers;
        private readonly HttpClient client;
        private readonly JsonSerializerSettings settings;
        private bool successOnly;

        static JsonService()
        {
            Headers = new ConcurrentDictionary<string, string>();
        }

        public JsonService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        }

        public JsonService(HttpClient client)
        {
            this.client = client;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        }

        public async Task<T> GetAsync<T>(string uri, object data = null)
        {
            var result = await GetAsync(uri, data);
            if (successOnly)
                result.EnsureSuccessStatusCode();
            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> GetAsync(string uri, object data = null)
        {
            var fullUri = uri + data.ToQueryString();
            var requestMessage = CreateRequest(HttpMethod.Get, fullUri);
            return await client.SendAsync(requestMessage);
        }

        public async Task<string> GetStringAsync(string uri, object data = null)
        {
            var fullUri = uri + data.ToQueryString();
            var requestMessage = CreateRequest(HttpMethod.Get, fullUri);
            var result = await client.SendAsync(requestMessage);
            if (successOnly)
                result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsStringAsync();
        }

        public async Task<T> PostAsync<T>(string uri, object data = null, bool dontSerialize = false)
        {
            var result = await PostAsync(uri, data, dontSerialize);
            if (successOnly)
                result.EnsureSuccessStatusCode();
            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, object data = null, bool dontSerialize = false)
        {
            var request = CreateRequest(HttpMethod.Post, uri);
            if (dontSerialize)
            {
                request.Content = new StringContent(data.SafeToString(), Encoding.UTF8, "application/json");
            }
            else
            {
                if (data != null)
                    request.Content = SerializeRequest(data);
            }
            return await client.SendAsync(request);
        }

        public async Task<T> PutAsync<T>(string uri, object data = null, bool dontSerialize = false)
        {
            var result = await PutAsync(uri, data, dontSerialize);
            if (successOnly)
                result.EnsureSuccessStatusCode();
            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> PutAsync(string uri, object data = null, bool dontSerialize = false)
        {
            var request = CreateRequest(HttpMethod.Put, uri);
            if (dontSerialize)
            {
                request.Content = new StringContent(data.SafeToString(), Encoding.UTF8, "application/json");
            }
            else
            {
                if (data != null)
                    request.Content = SerializeRequest(data);
            }
            return await client.SendAsync(request);
        }

        public async Task<T> DeleteAsync<T>(string uri)
        {
            var result = await DeleteAsync(uri);
            if (successOnly)
                result.EnsureSuccessStatusCode();
            return await DeserialiseResponse<T>(result);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var request = CreateRequest(HttpMethod.Delete, uri);
            request.Content = SerializeRequest();
            return await client.SendAsync(request);
        }

        public void SetHeader(string header, string value)
        {
            Headers[header] = value;
        }

        public void ClearHeader(string header)
        {
            Headers[header] = null;
        }

        public void ClearHeaders()
        {
            Headers.Clear();
        }

        public void EnableOnlySuccessOnlyMode(bool successOnly = true)
        {
            this.successOnly = successOnly;
        }

        public void Dispose()
        {
            client.Dispose();
        }

        private HttpContent SerializeRequest(object data = null)
        {
            StringContent request;
            if (data == null)
                request = new StringContent(string.Empty);
            else
                request = new StringContent(JsonConvert.SerializeObject(data, settings));
            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return request;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string uri)
        {
            var requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var headerValue in Headers.Where(xy => !string.IsNullOrEmpty(xy.Value)))
                requestMessage.Headers.Add(headerValue.Key, headerValue.Value);
            return requestMessage;
        }

        private async Task<T> DeserialiseResponse<T>(HttpResponseMessage result)
        {
            var payload = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(payload, settings);
        }
    }
}