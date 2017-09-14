using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentlyWindsor.EndersJson.Tests.Framework;
using NUnit.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace FluentlyWindsor.EndersJson.Tests
{
	[TestFixture]
    public class JsonServiceTests 
    {
	    private IWebHostBuilder CreateWebHostBuilder()
	    {
		    var config = new ConfigurationBuilder().Build();

		    var host = new WebHostBuilder()
			    .UseConfiguration(config)
			    .UseStartup<Startup>();

		    return host;
	    }

		[SetUp]
        public void SetUp()
        {
	        var webHostBuilder = CreateWebHostBuilder();
	        var server = new TestServer(webHostBuilder);
	        var httpClient = server.CreateClient();
			json = new JsonService(httpClient);
        }

	    [TearDown]
	    public void TearDown()
	    {
		    json.Dispose();
	    }

        private JsonService json;

        [Test]
        public async Task Should_be_able_to_DELETE_resource_from_web_api()
        {
            await json.DeleteAsync<Person>("api/person/1");
        }

        [Test]
        public async Task Should_be_able_to_DELETE_resource_from_web_api_return_OK()
        {
            var response = await json.DeleteAsync("api/person/1");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_DELETE_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.DeleteAsync<Person>("api/person_404/1");
            });
        }

        [Test]
        public async Task Should_be_able_to_DELETE_resource_from_web_api_if_not_found_return_NotFound()
        {
            var response = await json.DeleteAsync("api/person_404/1");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api()
        {
            var result = await json.GetAsync<IEnumerable<Person>>("api/persons");
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_return_OK()
        {
            var response = await json.GetAsync("api/persons");
	        var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Person>>(resultString);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Should_be_able_to_GET_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.GetAsync<IEnumerable<Person>>("api/persons_404");
            });
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_if_not_found_and_return_NotFound()
        {
            var response = await json.GetAsync("api/persons_404");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_using_authorisation_header()
        {
            json.SetHeader("Authorization", Guid.NewGuid().ToString("N"));
            var result = await json.GetAsync<IEnumerable<Person>>("api/persons");
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_using_authorisation_header_return_OK()
        {
            json.SetHeader("Authorization", Guid.NewGuid().ToString("N"));
            var response = await json.GetAsync("api/persons");
	        var resultString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<IEnumerable<Person>>(resultString);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Should_be_able_to_POST_resource_from_web_api()
        {
            var result = await json.PostAsync<Person>("api/person", Person.Any);
            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public async Task Should_be_able_to_POST_resource_from_web_api_return_Created()
        {
            var response = await json.PostAsync("api/person", Person.Any);
	        var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Person>(resultString);
            Assert.That(result, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void Should_be_able_to_POST_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.PostAsync<Person>("api/person_404", Person.Any);
            });
        }

        [Test]
        public async Task Should_be_able_to_POST_resource_from_web_api_if_not_found_return_NotFound()
        {
            var response = await json.PostAsync("api/person_404", Person.Any);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_PUT_resource_from_web_api()
        {
            await json.PutAsync<Person>("api/person", Person.Any);
        }

        [Test]
        public async Task Should_be_able_to_PUT_resource_from_web_api_return_OK()
        {
            var response = await json.PutAsync("api/person", Person.Any);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_PUT_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.PutAsync<Person>("api/person_404", Person.Any);
            });
        }

        [Test]
        public async Task Should_be_able_to_PUT_resource_from_web_api_if_not_found_return_NotFound()
        {
            var response = await json.PutAsync("api/person_404", Person.Any);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_GET_AS_STRING_from_web_api()
        {
            var result = await json.GetStringAsync("api/person", Person.Any);
            Assert.That(result, Is.TypeOf<string>());
            Assert.That(result, Is.Not.Null.Or.Empty);
        }
    }
}