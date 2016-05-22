using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// A delegating handler that automatically waits and retrieves when a 503 Service Unavailable response is received.
	/// </summary>
	public sealed class ServiceUnavailableRetryHandler : RetryHandlerBase
	{

		#region Constructors

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the chain to pass requests onto.</param>
		public ServiceUnavailableRetryHandler(HttpMessageHandler innerHandler) : base(innerHandler)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the chain to pass requests onto.</param>
		/// <param name="maxRetries">The maximum number of retries per initial request.</param>
		public ServiceUnavailableRetryHandler(HttpMessageHandler innerHandler, int maxRetries) : base(innerHandler, maxRetries)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the chain to pass requests onto.</param>
		/// <param name="maxRetries">The maximum number of retries per initial request.</param>
		/// <param name="maxPerRequestWaitTime">The maximum time to wait between retries. If the server requests a retry time greater than this the 503 response is returned an no wait/retry is performed. Specify <see cref="System.TimeSpan.Zero"/> for no limit.</param>
		public ServiceUnavailableRetryHandler(HttpMessageHandler innerHandler, int maxRetries, TimeSpan maxPerRequestWaitTime) : base(innerHandler, maxRetries, maxPerRequestWaitTime)
		{
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Returns true if the <paramref name="response"/> status code is <see cref="System.Net.HttpStatusCode.ServiceUnavailable"/>.
		/// </summary>
		/// <param name="response">A <see cref="HttpResponseMessage"/> to be analysed.</param>
		/// <returns>True if the request should be retried, otherwise false.</returns>
		protected override bool ShouldRetry(HttpResponseMessage response)
		{
			if (response == null) return true;

			return response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable;
		}

		#endregion

	}
}