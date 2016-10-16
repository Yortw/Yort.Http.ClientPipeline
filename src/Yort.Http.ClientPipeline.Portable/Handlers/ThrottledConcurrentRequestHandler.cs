using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// A delegating handler that restricts the number of concurrent requests, to avoid overloading network resources/bandwidth.
	/// </summary>
	/// <remarks>
	/// <para>Only requests made using the same instance of this handler will be throttled.</para>
	/// </remarks>
	public class ThrottledConcurrentRequestHandler : DelegatingHandler
	{
		/// <summary>
		/// Default constructor. Creates an instance that only allows 4 concurrent requests.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to pass the request onto.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="innerHandler"/> parameter is null.</exception>
		public ThrottledConcurrentRequestHandler(System.Net.Http.HttpMessageHandler innerHandler) : this(4, innerHandler)
		{
			Helper.Throw();
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
			Helper.Throw();
		}

		/// <summary>
		/// Passes on the request to the inner handler when there are less than the maximum number of concurrent requests, otherwise waits for the number of concurrent requests to drop below the maximum and then passes the request on.
		/// </summary>
		/// <param name="request">The request to pass on.</param>
		/// <param name="cancellationToken">The cancellation token that can be used to cancel the request.</param>
		/// <returns></returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		/// <param name="disposing"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_ThrottlingSemaphore", Justification="It is disposed, CA just doesn't understand ?. syntax.")]
		protected override void Dispose(bool disposing)
		{
			Helper.Throw();
		}
	}
}