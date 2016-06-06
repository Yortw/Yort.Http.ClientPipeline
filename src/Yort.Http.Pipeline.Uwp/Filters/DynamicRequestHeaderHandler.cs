using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Foundation;
using Windows.Web.Http.Filters;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Automatically applies headers with dynamic values to requests before passing them on.
	/// </summary>
	public sealed class DynamicRequestHeaderFilter : Windows.Web.Http.Filters.IHttpFilter
	{
		private IEnumerable<KeyValuePair<string, Func<string>>> _DynamicHeaders;
		private IWebHttpRequestCondition _ApplyHeadersCondition;
		private Windows.Web.Http.Filters.IHttpFilter _InnerFilter;

		/// <summary>
		/// Constructs a <see cref="DynamicRequestHeaderFilter"/>.
		/// </summary>
		/// <param name="dynamicHeaders">An enumerable set of <see cref="KeyValuePair{TKey, TValue}"/>s where each key is the name of a header and each value is a string returning function for the header value.</param>
		/// <param name="innerFilter">The next filter in the pipeline to pass requests to after the dynamic headers are added.</param>
		public DynamicRequestHeaderFilter(IEnumerable<KeyValuePair<string, Func<string>>> dynamicHeaders, Windows.Web.Http.Filters.IHttpFilter innerFilter) : this(dynamicHeaders, null, innerFilter)
		{
		}

		/// <summary>
		/// Constructs a <see cref="DynamicRequestHeaderFilter"/>.
		/// </summary>
		/// <param name="dynamicHeaders">An enumerable set of <see cref="KeyValuePair{TKey, TValue}"/>s where each key is the name of a header and each value is a string returning function for the header value.</param>
		/// <param name="applyHeadersCondition">A <see cref="IWebHttpRequestCondition"/> implementation used to determine if the dynamic headers should be applied. If null, headers are applied to all requests.</param>
		/// <param name="innerFilter">The next filter in the pipeline to pass requests to after the dynamic headers are added.</param>
		public DynamicRequestHeaderFilter(IEnumerable<KeyValuePair<string, Func<string>>> dynamicHeaders, IWebHttpRequestCondition applyHeadersCondition, Windows.Web.Http.Filters.IHttpFilter innerFilter) 
		{
			if (dynamicHeaders == null) throw new ArgumentNullException(nameof(dynamicHeaders));
			if (innerFilter == null) throw new ArgumentNullException(nameof(innerFilter));

			_InnerFilter = innerFilter;
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
		/// <param name="request">The <see cref="Windows.Web.Http.HttpRequestMessage"/> to process and pass on.</param>
		/// <returns>An asynchronous operation whose result is a <see cref="HttpResponseMessage"/> from the server.</returns>
		public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
		{
			return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
			{
				if (_ApplyHeadersCondition?.ShouldProcess(request) ?? true)
				{
					foreach (var kvp in _DynamicHeaders)
					{
						var value = kvp.Value();
						if (value != null) request.Headers.Add(kvp.Key, value);

						cancellationToken.ThrowIfCancellationRequested();
					}
				}

				return await _InnerFilter.SendRequestAsync(request).AsTask().ConfigureAwait(false);
			});
		}

		/// <summary>
		/// Disposes this object and all internal resources, including the inner filter.
		/// </summary>
		public void Dispose()
		{
			_InnerFilter?.Dispose();
		}
	}
}