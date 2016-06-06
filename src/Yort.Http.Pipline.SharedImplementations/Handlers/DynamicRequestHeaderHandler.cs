using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Automatically applies headers with dynamic values to requests before passing them on.
	/// </summary>
	public sealed class DynamicRequestHeaderHandler : DelegatingHandler
	{
		private IEnumerable<KeyValuePair<string, Func<string>>> _DynamicHeaders;
		private IRequestCondition _ApplyHeadersCondition;

		/// <summary>
		/// Constructs a <see cref="DynamicRequestHeaderHandler"/>.
		/// </summary>
		/// <param name="dynamicHeaders">An enumerable set of <see cref="KeyValuePair{TKey, TValue}"/>s where each key is the name of a header and each value is a string returning function for the header value.</param>
		/// <param name="innerHandler">The next handler in the pipeline to pass requests to after the dynamic headers are added.</param>
		public DynamicRequestHeaderHandler(IEnumerable<KeyValuePair<string, Func<string>>> dynamicHeaders, HttpMessageHandler innerHandler) : this(dynamicHeaders, null, innerHandler)
		{
		}

		/// <summary>
		/// Constructs a <see cref="DynamicRequestHeaderHandler"/>.
		/// </summary>
		/// <param name="dynamicHeaders">An enumerable set of <see cref="KeyValuePair{TKey, TValue}"/>s where each key is the name of a header and each value is a string returning function for the header value.</param>
		/// <param name="applyHeadersCondition">A <see cref="IRequestCondition"/> implementation used to determine if the dynamic headers should be applied. If null, headers are applied to all requests.</param>
		/// <param name="innerHandler">The next handler in the pipeline to pass requests to after the dynamic headers are added.</param>
		public DynamicRequestHeaderHandler(IEnumerable<KeyValuePair<string, Func<string>>> dynamicHeaders, IRequestCondition applyHeadersCondition,  HttpMessageHandler innerHandler) : base(innerHandler)
		{
			if (dynamicHeaders == null) throw new ArgumentNullException(nameof(dynamicHeaders));

			_ApplyHeadersCondition = applyHeadersCondition;
			_DynamicHeaders = dynamicHeaders;
		}

		/// <summary>
		/// Applies the configured dynamic headers and passes the request on.
		/// </summary>
		/// <remarks>
		/// <para>Does not modify headers that already have a value.</para>
		/// <para>Does not create/apply headers where the function returning the value returns null.</para>
		/// </remarks>
		/// <param name="request">The request to modify and pass on.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the request.</param>
		/// <returns>A task whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		/// <see cref="DynamicRequestHeaderHandler"/>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (_ApplyHeadersCondition?.ShouldProcess(request) ?? true)
			{
				foreach (var kvp in _DynamicHeaders)
				{
					var value = kvp.Value();
					if (value != null) request.Headers.TryAddWithoutValidation(kvp.Key, value);

					cancellationToken.ThrowIfCancellationRequested();
				}
			}

			return base.SendAsync(request, cancellationToken);
		}
	}
}