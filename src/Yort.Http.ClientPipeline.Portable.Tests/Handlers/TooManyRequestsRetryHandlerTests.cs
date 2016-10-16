using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yort.Http.ClientPipeline;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class TooManyRequestsRetryHandlerTests
	{
		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(TooManyRequestsRetryHandler))]
		[TestCategory("MessageHandlers")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TooManyRequestsRetryHandler_Constructor_ThrowsOnNullInnerHandler()
		{
			var handler = new TooManyRequestsRetryHandler(null);
		}

		[TestMethod]
		[TestCategory(nameof(TooManyRequestsRetryHandler))]
		[TestCategory("MessageHandlers")]
		public void TooManyRequestsRetryHandler_Constructor_ConstructsOkWithValidArguments()
		{
			using (var handler = new TooManyRequestsRetryHandler(new System.Net.Http.HttpClientHandler(), 5, TimeSpan.FromSeconds(1)))
			{
			}
		}

		#endregion

		#region SendRequestAsync Tests

		private int requestCount = 0;

		[TestMethod]
		[TestCategory(nameof(TooManyRequestsRetryHandler))]
		[TestCategory("MessageHandlers")]
		public async Task TooManyRequestsRetryHandler_SendRequestAsync_RetriesOnTooManyRequests()
		{
			var mockHandler = new MockMessageHandler();
			DateTime timeToStart = DateTime.Now.AddSeconds(1);

			var requestHandler = new MockResponseHandler()
			{
				CanHandleRequest = (request) => request.Method.Method == "GET" && request.RequestUri.ToString() == "http://testsite.com/test",
				HandleRequest = async (request) =>
				{
					try
					{
						System.Diagnostics.Debug.WriteLine(System.Threading.Interlocked.Increment(ref requestCount));

						var timeToWait = timeToStart.Subtract(DateTime.Now);
						if (timeToWait.TotalMilliseconds > 0)
							await Task.Delay(timeToWait).ConfigureAwait(false);

						if (requestCount < 4)
							return new System.Net.Http.HttpResponseMessage((System.Net.HttpStatusCode)429);
						else
							return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
					}
					finally
					{
						System.Threading.Interlocked.Increment(ref requestCount);
					}
				}
			};
			mockHandler.AddDynamicResponse(requestHandler);

			var handler = new TooManyRequestsRetryHandler(mockHandler, 5, TimeSpan.FromMilliseconds(1000));
			using (var client = new System.Net.Http.HttpClient(handler))
			{
				var response = await client.GetAsync("http://testsite.com/test");

				Assert.AreNotEqual(429, (int)response.StatusCode);
			}
		}

		#endregion

	}
}