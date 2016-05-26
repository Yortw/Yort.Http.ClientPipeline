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
	/// A delegating handler that automatically waits and retrieves based on response conditions.
	/// </summary>
	public abstract class RetryHandlerBase : DelegatingHandler
	{

		#region Fields

		private int _MaxRetries;
		private TimeSpan _MaxPerRequestWaitTime;

		private static readonly TimeSpan DefaultMaxRequestWaitTime = TimeSpan.FromMinutes(1);
		private const int DefaultMaxRetries = 5;

		#endregion

		#region Constructors

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the chain to pass requests onto.</param>
		protected RetryHandlerBase(HttpMessageHandler innerHandler) : this(innerHandler, DefaultMaxRetries)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the chain to pass requests onto.</param>
		/// <param name="maxRetries">The maximum number of retries per initial request.</param>
		protected RetryHandlerBase(HttpMessageHandler innerHandler, int maxRetries) : this(innerHandler, maxRetries, DefaultMaxRequestWaitTime)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the chain to pass requests onto.</param>
		/// <param name="maxRetries">The maximum number of retries per initial request.</param>
		/// <param name="maxPerRequestWaitTime">The maximum time to wait between retries. If the server requests a retry time greater than this the 503 response is returned an no wait/retry is performed. Specify <see cref="System.TimeSpan.Zero"/> for no limit.</param>
		protected RetryHandlerBase(HttpMessageHandler innerHandler, int maxRetries, TimeSpan maxPerRequestWaitTime) : base(innerHandler)
		{
			_MaxRetries = maxRetries;
			_MaxPerRequestWaitTime = maxPerRequestWaitTime;
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Sends the request and automatically waits/retries if the response is <see cref="System.Net.HttpStatusCode.ServiceUnavailable"/>.
		/// </summary>
		/// <param name="request">The <see cref="System.Net.Http.HttpRequestMessage"/> to send.</param>
		/// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the request.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpResponseMessage result = null;
			int retries = 0;
			while (retries < _MaxRetries || retries == 0)
			{
				result = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
				if (ShouldRetry(result))
				{
					if (await WaitIfNotTooLong(result.Headers.RetryAfter).ConfigureAwait(false))
						retries++;
					else
						break;
				}
				else
					break;
			}
			return result;
		}

		/// <summary>
		/// Abstract method called to determine if a retry is neccesary.
		/// </summary>
		/// <param name="response">A <see cref="HttpResponseMessage"/> to be analysed.</param>
		/// <returns>True if the request should be retried, otherwise false.</returns>
		protected abstract bool ShouldRetry(HttpResponseMessage response);

		#endregion

		#region Private Methods

		private async Task<bool> WaitIfNotTooLong(RetryConditionHeaderValue retryAfter)
		{
			var timeToWait = DefaultMaxRequestWaitTime;
			if (retryAfter != null)
			{
				if (retryAfter.Date != null)
					timeToWait = retryAfter.Date.Value.ToLocalTime().Subtract(DateTime.Now);
				else if (retryAfter.Delta != null)
					timeToWait = retryAfter.Delta.Value;
			}

			if (timeToWait.TotalMilliseconds > 0 && (timeToWait <= _MaxPerRequestWaitTime || _MaxPerRequestWaitTime == TimeSpan.Zero))
			{
#if SUPPORTS_TASKEX
				await TaskEx.Delay(timeToWait).ConfigureAwait(false);
#else
				await Task.Delay(timeToWait).ConfigureAwait(false);
#endif
				return true;
			}

			return false;
		}

		#endregion

	}
}