using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// A delegating handler that restricts the number of concurrent requests, to avoid overloading network resources/bandwidth.
	/// </summary>
	/// <remarks>
	/// <para>Only requests made using the same instance of this handler will be throttled.</para>
	/// </remarks>
	public class ThrottledConcurrentRequestHandler : DelegatingHandler
	{
		private System.Threading.Semaphore _ThrottlingSemaphore;

		private const int DefaultMaxRequests = 4;

		/// <summary>
		/// Default constructor. Creates an instance that only allows 4 concurrent requests.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to pass the request onto.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="innerHandler"/> parameter is null.</exception>
		public ThrottledConcurrentRequestHandler(System.Net.Http.HttpMessageHandler innerHandler) : this(DefaultMaxRequests, innerHandler)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="maxConcurrentRequests">The maximum number of concurrent requests allowed. Must be greater than zero</param>
		/// <param name="innerHandler">The next handler in the pipeline to pass the request onto.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="maxConcurrentRequests"/> is less than or equal to zero.</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="innerHandler"/> parameter is null.</exception>
		public ThrottledConcurrentRequestHandler(int maxConcurrentRequests, System.Net.Http.HttpMessageHandler innerHandler) : base(innerHandler)
		{
			if (maxConcurrentRequests <= 0) throw new ArgumentOutOfRangeException(nameof(maxConcurrentRequests));
			if (innerHandler == null) throw new ArgumentNullException(nameof(innerHandler));

			_ThrottlingSemaphore = new System.Threading.Semaphore(maxConcurrentRequests, maxConcurrentRequests);
		}

		/// <summary>
		/// Passes on the request to the inner handler when there are less than the maximum number of concurrent requests, otherwise waits for the number of concurrent requests to drop below the maximum and then passes the request on.
		/// </summary>
		/// <param name="request">The request to pass on.</param>
		/// <param name="cancellationToken">The cancellation token that can be used to cancel the request.</param>
		/// <returns></returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var semaphore = _ThrottlingSemaphore;
			if (semaphore == null) throw new ObjectDisposedException(nameof(ThrottledConcurrentRequestHandler));

			while (!semaphore.WaitOne(250))
			{
				cancellationToken.ThrowIfCancellationRequested();
			}

			try
			{
				return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
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
		}

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		/// <param name="disposing"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_ThrottlingSemaphore", Justification="It is disposed, CA just doesn't understand ?. syntax.")]
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_ThrottlingSemaphore?.Dispose();
				_ThrottlingSemaphore = null;
			}

			base.Dispose(disposing);
		}
	}
}