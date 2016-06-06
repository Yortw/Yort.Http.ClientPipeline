using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class DynamicRequestHeaderHandlerTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(DynamicRequestHeaderHandler))]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void DynamicRequestHeaderHandler_Constructor_ThrowsOnNullHeaders()
		{
			var handler = new DynamicRequestHeaderHandler(null, new System.Net.Http.HttpClientHandler());
		}

		[TestMethod]
		[TestCategory(nameof(DynamicRequestHeaderHandler))]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void DynamicRequestHeaderHandler_Constructor_ThrowsOnNullInnerHandler()
		{
			var handler = new DynamicRequestHeaderHandler(new KeyValuePair<string, Func<string>>[] { }, null);
		}

		[TestMethod]
		[TestCategory(nameof(DynamicRequestHeaderHandler))]
		public void DynamicRequestHeaderHandler_Constructor_AllowsNullFilter()
		{
			var handler = new DynamicRequestHeaderHandler(new KeyValuePair<string, Func<string>>[] { }, null, new System.Net.Http.HttpClientHandler());
		}

		[TestMethod]
		[TestCategory(nameof(DynamicRequestHeaderHandler))]
		public void DynamicRequestHeaderHandler_Constructor_ConstructsOkWithNoNullArguments()
		{
			var handler = new DynamicRequestHeaderHandler(new KeyValuePair<string, Func<string>>[] { }, new System.Net.Http.HttpClientHandler());
		}

		#endregion

		#region DynamicRequestHeaderHandler SendRequest Tests

		[TestMethod]
		[TestCategory(nameof(DynamicRequestHeaderHandler))]
		public async Task DynamicRequestHeaderHandler_SendRequest_AppliesDynamicHeaders()
		{
			var mockHandler = new MockMessageHandler()
			{
				DefaultResponse = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NoContent)
			};

			var dynamicHeaders = new Dictionary<string, Func<string>>();
			dynamicHeaders.Add("TestHeader", () => System.Guid.NewGuid().ToString());
			var handler = new DynamicRequestHeaderHandler(dynamicHeaders, mockHandler);
			var client = new HttpClient(handler);

			await TestDynamicHeadersApplied(mockHandler, client);
		}

		[TestMethod]
		[TestCategory(nameof(DynamicRequestHeaderHandler))]
		public async Task DynamicRequestHeaderHandler_SendRequest_WithPassingFilter_AppliesDynamicHeaders()
		{
			var mockHandler = new MockMessageHandler()
			{
				DefaultResponse = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NoContent)
			};

			var filter = new AuthorityRequestCondition(new string[] { "sometestsite.com" });
			var dynamicHeaders = new Dictionary<string, Func<string>>();
			dynamicHeaders.Add("TestHeader", () => System.Guid.NewGuid().ToString());
			var handler = new DynamicRequestHeaderHandler(dynamicHeaders, filter, mockHandler);
			var client = new HttpClient(handler);

			await TestDynamicHeadersApplied(mockHandler, client);
		}

		[TestMethod]
		[TestCategory(nameof(DynamicRequestHeaderHandler))]
		public async Task DynamicRequestHeaderHandler_SendRequest_WithFailingFilter_DoesNotApplyDynamicHeaders()
		{
			var mockHandler = new MockMessageHandler()
			{
				DefaultResponse = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NoContent)
			};

			var filter = new AuthorityRequestCondition(new string[] { "someothertestsite" });
			var dynamicHeaders = new Dictionary<string, Func<string>>();
			dynamicHeaders.Add("TestHeader", () => System.Guid.NewGuid().ToString());
			var handler = new DynamicRequestHeaderHandler(dynamicHeaders, filter, mockHandler);
			var client = new HttpClient(handler);

			var result = await client.GetAsync("http://sometestsite.com/SomeEndPoint");

			Assert.IsFalse(mockHandler.Requests.Last().Headers.Contains("TestHeader"));
		}

		#endregion

		#region Private Methods

		private static async Task TestDynamicHeadersApplied(MockMessageHandler mockHandler, HttpClient client)
		{
			var result = await client.GetAsync("http://sometestsite.com/SomeEndPoint");

			Assert.IsTrue(mockHandler.Requests.Last().Headers.Contains("TestHeader"));
			var values = mockHandler.Requests.Last().Headers.GetValues("TestHeader");
			Assert.IsNotNull(values);
			Assert.AreEqual(1, values.Count());
			var firstValue = Guid.Parse(values.First());

			result = await client.GetAsync("http://sometestsite.com/SomeEndPoint");
			Assert.IsTrue(mockHandler.Requests.Last().Headers.Contains("TestHeader"));
			values = mockHandler.Requests.Last().Headers.GetValues("TestHeader");
			Assert.IsNotNull(values);
			Assert.AreEqual(1, values.Count());
			var secondValue = Guid.Parse(values.First());

			Assert.AreNotEqual(firstValue, secondValue);
		}

		#endregion

	}
}