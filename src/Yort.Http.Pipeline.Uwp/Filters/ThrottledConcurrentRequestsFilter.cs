using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// A delegating handler that restricts the number of concurrent requests, to avoid overloading network resources/bandwidth.
	/// </summary>
	/// <remarks>
	/// <para>Only requests made using the same instance of this handler will be throttled.</para>
	/// </remarks>
	public sealed class ThrottledConcurrentRequestsFilter : IHttpFilter
	{
		private System.Threading.Semaphore _ThrottlingSemaphore;
		private IHttpFilter _InnerHandler;

		private const int DefaultMaxRequests = 4;

		/// <summary>
		/// Default constructor. Creates an instance that only allows 4 concurrent requests.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to pass the request onto.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="innerHandler"/> parameter is null.</exception>
		public ThrottledConcurrentRequestsFilter(IHttpFilter innerHandler) : this(DefaultMaxRequests, innerHandler)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="maxConcurrentRequests">The maximum number of concurrent requests allowed. Must be greater than zero</param>
		/// <param name="innerHandler">The next handler in the pipeline to pass the request onto.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="maxConcurrentRequests"/> is less than or equal to zero.</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="innerHandler"/> parameter is null.</exception>
		public ThrottledConcurrentRequestsFilter(int maxConcurrentRequests, IHttpFilter innerHandler)
		{
			if (maxConcurrentRequests <= 0) throw new ArgumentOutOfRangeException(nameof(maxConcurrentRequests));
			if (innerHandler == null) throw new ArgumentNullException(nameof(innerHandler));

			_ThrottlingSemaphore = new System.Threading.Semaphore(maxConcurrentRequests, maxConcurrentRequests);
			_InnerHandler = innerHandler;
		}

		/// <summary>
		/// Passes on the request to the inner handler when there are less than the maximum number of concurrent requests, otherwise waits for the number of concurrent requests to drop below the maximum and then passes the request on.
		/// </summary>
		/// <param name="request">The request to pass on.</param>
		/// <returns>An asynchronous operation whose result is a <see cref="HttpResponseMessage"/>.</returns>
		public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
		{
			return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
			{
				var semaphore = _ThrottlingSemaphore;
				if (semaphore == null) throw new ObjectDisposedException(nameof(ThrottledConcurrentRequestHandler));

				while (!semaphore.WaitOne(250))
				{
					cancellationToken.ThrowIfCancellationRequested();
				}

				try
				{
					return await _InnerHandler.SendRequestAsync(request).AsTask().ConfigureAwait(false);
				}
				finally
				{
					try
					{
						//If the handler was disposed while the request was inflihght, avoid throwing
						//the object disposed condition. However, a race condition is still possible
						//so catch and suppress the exception if it does occur.
						if (_ThrottlingSemaphore != null)
							semaphore.Release();
					}
					catch (ObjectDisposedException) { }
				}
			});
		}

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_ThrottlingSemaphore", Justification = "It is disposed, CA just doesn't understand ?. syntax.")]
		public void Dispose()
		{
			_ThrottlingSemaphore?.Dispose();
			_ThrottlingSemaphore = null;
		}
	}
}