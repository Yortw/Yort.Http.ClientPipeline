using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class ThrottledConcurrentRequestHandlerTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(ThrottledConcurrentRequestHandler))]
		[TestCategory("MessageHandlers")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrottledConcurrentRequestHandler_Constructor_ThrowsOnNullInnerHandler()
		{
			var handler = new ThrottledConcurrentRequestHandler(null);
		}

		[TestMethod]
		[TestCategory(nameof(ThrottledConcurrentRequestHandler))]
		[TestCategory("MessageHandlers")]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ThrottledConcurrentRequestHandler_Constructor_ThrowsOnZeroMaxConcurrentRequests()
		{
			var handler = new ThrottledConcurrentRequestHandler(0, new System.Net.Http.HttpClientHandler());
		}

		[TestMethod]
		[TestCategory(nameof(ThrottledConcurrentRequestHandler))]
		[TestCategory("MessageHandlers")]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ThrottledConcurrentRequestHandler_Constructor_ThrowsOnNegativeMaxConcurrentRequests()
		{
			var handler = new ThrottledConcurrentRequestHandler(-1, new System.Net.Http.HttpClientHandler());
		}

		[TestMethod]
		[TestCategory(nameof(ThrottledConcurrentRequestHandler))]
		[TestCategory("MessageHandlers")]
		public void ThrottledConcurrentRequestHandler_Constructor_ConstructsOkWithValidArguments()
		{
			using (var handler = new ThrottledConcurrentRequestHandler(6, new System.Net.Http.HttpClientHandler()))
			{
			}
		}

		#endregion

		#region SendRequestAsync Tests

		private int concurrentRequests = 0;

		[TestMethod]
		[TestCategory(nameof(ThrottledConcurrentRequestHandler))]
		[TestCategory("MessageHandlers")]
		public void ThrottledConcurrentRequestHandler_SendRequestAsync_ThrottlesRequests()
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
						System.Diagnostics.Debug.WriteLine(System.Threading.Interlocked.Increment(ref concurrentRequests));

						var timeToWait = timeToStart.Subtract(DateTime.Now);
						if (timeToWait.TotalMilliseconds > 0)
							await Task.Delay(timeToWait).ConfigureAwait(false);

						if (concurrentRequests > 4)
							return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
						else
							return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
					}
					finally
					{
						System.Threading.Interlocked.Decrement(ref concurrentRequests);
					}
				}
			};
			mockHandler.AddDynamicResponse(requestHandler);

			var handler = new ThrottledConcurrentRequestHandler(mockHandler);
			using (var client = new System.Net.Http.HttpClient(handler))
			{
				var requests = new List<Task<System.Net.Http.HttpResponseMessage>>();
				for (int cnt = 0; cnt < 100; cnt++)
				{
					requests.Add(client.GetAsync("http://testsite.com/test"));
				}
				Task.WaitAll(requests.ToArray());

				Assert.IsFalse((from r in requests where r.Result.StatusCode == System.Net.HttpStatusCode.InternalServerError select r).Any());
			}
		}

		#endregion

	}
}