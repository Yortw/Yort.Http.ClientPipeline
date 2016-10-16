using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class MockMessageHandlerTests
	{
		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(MockMessageHandler))]
		public async Task MockMessageHandler_ReturnsNotFoundForUnknownRequest()
		{
			var requestUriString = "http://www.mytestdomain.com/";

			var handler = new MockMessageHandler();
			var client = new System.Net.Http.HttpClient(handler);
			var result = await client.GetAsync(requestUriString);

			Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result.StatusCode);
			Assert.AreEqual(requestUriString, result.RequestMessage.RequestUri.ToString());
		}

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(MockMessageHandler))]
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

			var lastRequest = handler.Requests.Last();
			Assert.AreEqual("GET", lastRequest.Method.Method);
			Assert.AreEqual(requestUriString, lastRequest.RequestUri.ToString());
		}

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(MockMessageHandler))]
		public async Task MockMessageHandler_ReturnsDynamicResponse()
		{
			var requestUriString = "http://www.mytestdomain.com/Greeting";

			var handler = new MockMessageHandler();
			handler.AddDynamicResponse(
				new MockResponseHandler()
				{
					// This decides if we will handle this specific request.
					CanHandleRequest = (request) =>
					{
						return request.RequestUri.ToString() == requestUriString
						 && String.Compare(request.Method.Method, "POST", true) == 0; // Only handle POST requests to our end test endpoint
					},

					// This actually handles the request.
					HandleRequest = async (request) =>
					{
						// Read the text passed in the request and return it as part 
						// of our dynamic response.
						var name = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
						var retVal = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);
						retVal.Content = new System.Net.Http.StringContent("Hello " + name);

						return retVal;
					}
				}
			);

			var client = new System.Net.Http.HttpClient(handler);
			var result = await client.PostAsync(requestUriString, new System.Net.Http.StringContent("Yort"));

			Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
			Assert.AreEqual("Hello Yort", await result.Content.ReadAsStringAsync());
		}

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(MockMessageHandler))]
		public async Task MockMessageHandler_UsesDefaultResponse()
		{
			// This is the end point we'll GET to, but we won't register
			// it with the handler, so it will be unhandled and result in
			// the default response.
			var requestUriString = "http://www.mytestdomain.com/";

			// This is the location we're going to send back as a redirect to
			// whenever any unhandled request is received.
			var notFoundResponseUri = new Uri("http://www.mytestdomain.com/ohnoes.html");

			// Here we create the handler and set the default response.
			var handler = new MockMessageHandler();
			handler.DefaultResponse = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Redirect);
			handler.DefaultResponse.Headers.Location = notFoundResponseUri;

			// Now create a client and make the call, which will result in a copy of the 
			// default response being returned.
			var client = new System.Net.Http.HttpClient(handler);
			var result = await client.GetAsync(requestUriString);

			Assert.AreEqual(System.Net.HttpStatusCode.Redirect, result.StatusCode);
			Assert.AreEqual(notFoundResponseUri, result.Headers.Location);
		}

	}
}