using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// A message handler that will automatically follow redirect responses for GET requests regardless of whether they change from HTTPS to HTTP.
	/// </summary>
	/// <remarks>
	/// <para>This handler follows redirects regardless of whether the redirect uses the same protocol (HTTPS vs HTTP) as the original request, but only for GET requests.
	/// It is primarily designed for following shortened links, where often the link shortener always provides an HTTPS url regardless of the original protocol. In this case
	/// the loss of security when changing from HTTPS to HTTP should not be of great concern as the original URL didn't require security anyway. However, non GET requests are
	/// ignored by this handler as the client may be sending sensitive information and unaware of the insecure redirect.</para>
	/// <para>This handler also keeps an in memory list of permanent redirects it has already followed, so it the same URL's are requested repeatedly the handler can skip 
	/// to the redirect URL without actually making a network request on subsequent calls.</para>
	/// </remarks>
	public sealed class InsecureRedirectionFilter : Windows.Web.Http.Filters.IHttpFilter
	{

		private const int DefaultMaxRedirections = 50;

		private Windows.Web.Http.Filters.IHttpFilter _InnerFilter;
		private int _MaxRedirects = DefaultMaxRedirections;
		private IRedirectCache _RedirectCache;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the pipeline to process requests.</param>
		public InsecureRedirectionFilter(IHttpFilter innerFilter) : this(innerFilter, DefaultMaxRedirections)
		{
		}

		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the pipeline to process requests.</param>
		/// <param name="maxRedirects">The maximum number of automatic redirections for the handler to follow per request.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="innerFilter"/> is null.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="maxRedirects"/> is less than zero.</exception>
		public InsecureRedirectionFilter(IHttpFilter innerFilter, int maxRedirects) : this(innerFilter, maxRedirects, new RedirectCache())
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the pipeline to process requests.</param>
		/// <param name="maxRedirects">The maximum number of automatic redirections for the handler to follow per request.</param>
		/// <param name="redirectCache">A <see cref="RedirectCache"/> instance to use for caching known redirects.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="innerFilter"/> is null.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="maxRedirects"/> is less than zero.</exception>
		public InsecureRedirectionFilter(IHttpFilter innerFilter, int maxRedirects, IRedirectCache redirectCache) 
		{
			if (innerFilter == null) throw new ArgumentNullException(nameof(innerFilter));
			if (maxRedirects < 0) throw new ArgumentOutOfRangeException(nameof(maxRedirects));

			_InnerFilter = innerFilter;
			MaxRedirects = maxRedirects;
			_RedirectCache = redirectCache ?? new RedirectCache();
		}

		/// <summary>
		/// Gets or sets the maximum number of automatic redirections per request.
		/// </summary>
		/// <remarks>
		/// <para>The default value is 50.</para>
		/// </remarks>
		public int MaxRedirects
		{
			get
			{
				return _MaxRedirects;
			}

			set
			{
				_MaxRedirects = value;
			}
		}
		/// <summary>
		/// Checks for known redirects and alters the request url if required, passes the request on to the next handler in the pipeline, then handles any redirect responses until a non-redirect response is received.
		/// </summary>
		/// <param name="request">The <see cref="HttpRequestMessage"/> to send.</param>
		/// <returns></returns>
		public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
		{
			return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
			{
				request.RequestUri = _RedirectCache.GetFinalUri(request.RequestUri);
				cancellationToken.ThrowIfCancellationRequested();

				var result = await _InnerFilter.SendRequestAsync(request).AsTask().ConfigureAwait(false);

				int redirects = 0;
				while (redirects < MaxRedirects)
				{
					if (result.StatusCode.IsRedirect())
					{
						if (String.Compare(request.Method.Method, "GET", StringComparison.OrdinalIgnoreCase) == 0 && result.Headers.Location != null)
						{
							if (result.StatusCode.IsPermanentRedirect())
								_RedirectCache.AddOrUpdateRedirect(request.RequestUri, result.Headers.Location);

							request.RequestUri = result.Headers.Location;
							result = await _InnerFilter.SendRequestAsync(request).AsTask().ConfigureAwait(false);
						}
						redirects++;
					}
					else
						break;
				}
				return result;
			});
		}

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		public void Dispose()
		{
			_InnerFilter.Dispose();
		}
	}
}