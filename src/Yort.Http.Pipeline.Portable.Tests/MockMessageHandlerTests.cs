using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class MockMessageHandlerTests
	{
		[TestMethod]
		public async Task MockMessageHandler_ReturnsNotFoundForUknownRequest()
		{
			var requestUriString = "http://www.mytestdomain.com/";

			var handler = new MockMessageHandler();
			var client = new System.Net.Http.HttpClient(handler);
			var result = await client.GetAsync(requestUriString);

			Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result.StatusCode);
			Assert.AreEqual(requestUriString, result.RequestMessage.RequestUri.ToString());
		}

		[TestMethod]
		public async Task MockMessageHandler_ReturnsFixedResponse()
		{
			var requestUriString = "http://www.mytestdomain.com/";

			var responseContentString = "Hello World!";
			var responseMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new System.Net.Http.StringContent(responseContentString)
			};

			var handler = new MockMessageHandler();
			handler.AddFixedResponse(new Uri(requestUriString), responseMessage);

			var client = new System.Net.Http.HttpClient(handler);
			var result = await client.GetAsync(requestUriString);

			Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
			Assert.AreEqual(responseContentString, await result.Content.ReadAsStringAsync());
		}

	}
}