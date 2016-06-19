using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.Web.Http.Filters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// A base class for filters that retry based on response conditions.
	/// </summary>
	public abstract class RetryFilterBase : IHttpFilter
	{

		#region Fields

		private IHttpFilter _InnerFilter;
		private int _MaxRetries;
		private TimeSpan _MaxPerRequestWaitTime;

		private static readonly TimeSpan DefaultMaxRequestWaitTime = TimeSpan.FromMinutes(1);
		private const int DefaultMaxRetries = 5;

		#endregion

		#region Constructors

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the chain to pass requests onto.</param>
		protected RetryFilterBase(IHttpFilter innerFilter) : this(innerFilter, DefaultMaxRetries)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the chain to pass requests onto.</param>
		/// <param name="maxRetries">The maximum number of retries per initial request.</param>
		protected RetryFilterBase(IHttpFilter innerFilter, int maxRetries) : this (innerFilter, maxRetries, DefaultMaxRequestWaitTime) 
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the chain to pass requests onto.</param>
		/// <param name="maxRetries">The maximum number of retries per initial request.</param>
		/// <param name="maxPerRequestWaitTime">The maximum time to wait between retries. If the server requests a retry time greater than this the 503 response is returned an no wait/retry is performed. Specify <see cref="System.TimeSpan.Zero"/> for no limit.</param>
		protected RetryFilterBase(IHttpFilter innerFilter, int maxRetries, TimeSpan maxPerRequestWaitTime) 
		{
			_MaxRetries = maxRetries;
			_MaxPerRequestWaitTime = maxPerRequestWaitTime;
			_InnerFilter = innerFilter;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sends the request and automatically waits/retries if the response is <see cref="Windows.Web.Http.HttpStatusCode.ServiceUnavailable"/>.
		/// </summary>
		/// <param name="request">The <see cref="Windows.Web.Http.HttpRequestMessage"/> to send.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="Windows.Web.Http.HttpResponseMessage"/> from the server.</returns>
		public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
		{
			return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
			{
				HttpResponseMessage result = null;
				int retries = 0;
				while (retries < _MaxRetries || retries == 0)
				{
					result = await _InnerFilter.SendRequestAsync(request).AsTask().ConfigureAwait(false);
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
			});
		}

		/// <summary>
		/// Returns true if the <paramref name="response"/> status code is <see cref="System.Net.HttpStatusCode.ServiceUnavailable"/>.
		/// </summary>
		/// <param name="response">A <see cref="HttpResponseMessage"/> to be analysed.</param>
		/// <returns>True if the request should be retried, otherwise false.</returns>
		protected abstract bool ShouldRetry(HttpResponseMessage response);

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		/// <param name="isDisposing">True if this instance is being explicitly disposed, false if it is being disposed from a finaliser.</param>
		protected virtual void Dispose(bool isDisposing)
		{
			if (isDisposing)
				_InnerFilter?.Dispose();
		}

		#endregion

		#region Private Methods

		private async Task<bool> WaitIfNotTooLong(HttpDateOrDeltaHeaderValue retryAfter)
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

		#region IDisposable Members

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Dispose(true);
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}

		#endregion

	}
}