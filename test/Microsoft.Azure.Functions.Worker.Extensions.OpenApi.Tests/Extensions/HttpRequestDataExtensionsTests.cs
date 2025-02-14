using System;

using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class HttpRequestDataExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_Queries_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiHttpRequestDataExtensions.Queries(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_NullQuerystring_When_Queries_Invoked_Then_It_Should_Return_Result()
        {
            var context = new Mock<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri);

            var result = OpenApiHttpRequestDataExtensions.Queries(req);

            result.Count.Should().Be(0);
       }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Given_NoQuerystring_When_Queries_Invoked_Then_It_Should_Return_Result(string querystring)
        {
            var context = new Mock<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri);

            var result = OpenApiHttpRequestDataExtensions.Queries(req);

            result.Count.Should().Be(0);
       }

        [DataTestMethod]
        [DataRow("hello=world", "hello")]
        [DataRow("hello=world&lorem=ipsum", "hello", "lorem")]
        public void Given_Querystring_When_Queries_Invoked_Then_It_Should_Return_Result(string querystring, params string[] keys)
        {
            var context = new Mock<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri);

            var result = OpenApiHttpRequestDataExtensions.Queries(req);

            result.Count.Should().Be(keys.Length);
            result.Keys.Should().Contain(keys);
        }

        [TestMethod]
        public void Given_Null_When_Query_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiHttpRequestDataExtensions.Query(null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello=world", "hello", "world")]
        [DataRow("hello=world&lorem=ipsum", "hello", "world")]
        [DataRow("hello=world&lorem=ipsum", "lorem", "ipsum")]
        public void Given_Querystring_When_Query_Invoked_Then_It_Should_Return_Result(string querystring, string key, string expected)
        {
            var context = new Mock<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri);

            var result = (string) OpenApiHttpRequestDataExtensions.Query(req, key);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_NullQuerystring_When_Query_Invoked_Then_It_Should_Return_Result()
        {
            var context = new Mock<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri);

            var result = (string) OpenApiHttpRequestDataExtensions.Query(req, "hello");

            result.Should().BeNull();
       }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Given_NoQuerystring_When_Query_Invoked_Then_It_Should_Return_Result(string querystring)
        {
            var context = new Mock<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri);

            var result = (string) OpenApiHttpRequestDataExtensions.Query(req, "hello");

            result.Should().BeNull();
       }
    }
}
