using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Automatically (gzip) compresses request contents sets the content encoding header to gzip.
	/// </summary>
	public sealed class CompressedRequestHandler : System.Net.Http.DelegatingHandler
	{
		#region Constructors

		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to pass requests through.</param>
		public CompressedRequestHandler(System.Net.Http.HttpMessageHandler innerHandler) : this(innerHandler, null)
		{
			Helper.Throw();
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to pass requests through.</param>
		/// <param name="requestCondition">A <see cref="IRequestCondition"/> that controls whether any individual request is compressed or not. If null, all requests are compressed.</param>
		public CompressedRequestHandler(System.Net.Http.HttpMessageHandler innerHandler, IRequestCondition requestCondition) : base(innerHandler)
		{
			Helper.Throw();
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Compresses the content if suitable, sets the content encoding header and passes on the request to the next handler in the pipeline.
		/// </summary>
		/// <param name="request">The <see cref="System.Net.Http.HttpRequestMessage"/> to send.</param>
		/// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the request.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
		}

		#endregion

	}
}