﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentlyWindsor.EndersJson.Tests.Framework;
using NUnit.Framework;

namespace FluentlyWindsor.EndersJson.Tests
{
    [TestFixture]
    public class JsonServiceTests : WebApiTestBase
    {
        [SetUp]
        public void SetUp()
        {
            json = new JsonService();
        }

        private JsonService json;

        [Test]
        public async Task Should_be_able_to_DELETE_resource_from_web_api()
        {
            await json.DeleteAsync<Person>(FormatUri("api/person/1"));
        }

        [Test]
        public async Task Should_be_able_to_DELETE_resource_from_web_api_return_OK()
        {
            var response = await json.DeleteAsync(FormatUri("api/person/1"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_DELETE_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.DeleteAsync<Person>(FormatUri("api/person_404/1"));
            });
        }

        [Test]
        public async Task Should_be_able_to_DELETE_resource_from_web_api_if_not_found_return_NotFound()
        {
            var response = await json.DeleteAsync(FormatUri("api/person_404/1"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api()
        {
            var result = await json.GetAsync<IEnumerable<Person>>(FormatUri("api/persons"));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_return_OK()
        {
            var response = await json.GetAsync(FormatUri("api/persons"));
            var result = await response.Content.ReadAsAsync<IEnumerable<Person>>();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Should_be_able_to_GET_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.GetAsync<IEnumerable<Person>>(FormatUri("api/persons_404"));
            });
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_if_not_found_and_return_NotFound()
        {
            var response = await json.GetAsync(FormatUri("api/persons_404"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_using_authorisation_header()
        {
            json.SetHeader("Authorization", Guid.NewGuid().ToString("N"));
            var result = await json.GetAsync<IEnumerable<Person>>(FormatUri("api/persons"));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_using_authorisation_header_return_OK()
        {
            json.SetHeader("Authorization", Guid.NewGuid().ToString("N"));
            var response = await json.GetAsync(FormatUri("api/persons"));
            var result = await response.Content.ReadAsAsync<IEnumerable<Person>>();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Should_be_able_to_POST_resource_from_web_api()
        {
            var result = await json.PostAsync<Person>(FormatUri("api/person"), Person.Any);
            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public async Task Should_be_able_to_POST_resource_from_web_api_return_Created()
        {
            var response = await json.PostAsync(FormatUri("api/person"), Person.Any);
            var result = await response.Content.ReadAsAsync<Person>();
            Assert.That(result, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void Should_be_able_to_POST_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.PostAsync<Person>(FormatUri("api/person_404"), Person.Any);
            });
        }

        [Test]
        public async Task Should_be_able_to_POST_resource_from_web_api_if_not_found_return_NotFound()
        {
            var response = await json.PostAsync(FormatUri("api/person_404"), Person.Any);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_PUT_resource_from_web_api()
        {
            await json.PutAsync<Person>(FormatUri("api/person"), Person.Any);
        }

        [Test]
        public async Task Should_be_able_to_PUT_resource_from_web_api_return_OK()
        {
            var response = await json.PutAsync(FormatUri("api/person"), Person.Any);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_PUT_resource_from_web_api_and_throw_if_not_found()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                json.EnableOnlySuccessOnlyMode();
                await json.PutAsync<Person>(FormatUri("api/person_404"), Person.Any);
            });
        }

        [Test]
        public async Task Should_be_able_to_PUT_resource_from_web_api_if_not_found_return_NotFound()
        {
            var response = await json.PutAsync(FormatUri("api/person_404"), Person.Any);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Should_be_able_to_GET_AS_STRING_from_web_api()
        {
            var result = await json.GetStringAsync(FormatUri("api/person"), Person.Any);
            Assert.That(result, Is.TypeOf<string>());
            Assert.That(result, Is.Not.Null.Or.Empty);
        }
    }
}