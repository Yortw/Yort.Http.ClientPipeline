using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace Yort.Http.Pipeline
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
	public sealed class InsecureRedirectionHandler : DelegatingHandler
	{

		#region Fields

		private const int DefaultMaxRedirections = 50;

		private int _MaxRedirects = DefaultMaxRedirections;
		private IRedirectCache _RedirectCache;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to process requests.</param>
		public InsecureRedirectionHandler(HttpMessageHandler innerHandler) : this(innerHandler, DefaultMaxRedirections)
		{
		}

		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to process requests.</param>
		/// <param name="maxRedirects">The maximum number of automatic redirections for the handler to follow per request.</param>
		public InsecureRedirectionHandler(HttpMessageHandler innerHandler, int maxRedirects) : this(innerHandler, maxRedirects, new RedirectCache()) 
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerHandler">The next handler in the pipeline to process requests.</param>
		/// <param name="maxRedirects">The maximum number of automatic redirections for the handler to follow per request.</param>
		/// <param name="redirectCache">A <see cref="RedirectCache"/> instance to use for caching known redirects.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="innerHandler"/> is null.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="maxRedirects"/> is less than zero.</exception>
		public InsecureRedirectionHandler(HttpMessageHandler innerHandler, int maxRedirects, IRedirectCache redirectCache) : base(innerHandler)
		{
			if (innerHandler == null) throw new ArgumentNullException(nameof(innerHandler));
			if (maxRedirects < 0) throw new ArgumentOutOfRangeException(nameof(maxRedirects));

			MaxRedirects = maxRedirects;
			_RedirectCache = redirectCache ?? new RedirectCache();
		}

		#endregion

		#region Public Properties

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

		#endregion

		#region Overrides

		/// <summary>
		/// Checks for known redirects and alters the request url if required, passes the request on to the next handler in the pipeline, then handles any redirect responses until a non-redirect response is received.
		/// </summary>
		/// <param name="request">The <see cref="HttpRequestMessage"/> to send.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the request.</param>
		/// <returns></returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			request.RequestUri = _RedirectCache.GetFinalUri(request.RequestUri);
			cancellationToken.ThrowIfCancellationRequested();

			var result = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

			int redirects = 0;
			while (redirects < MaxRedirects)
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (result.StatusCode.IsRedirect())
				{
					if (String.Compare(request.Method.Method, "GET", StringComparison.OrdinalIgnoreCase) == 0 && result.Headers.Location != null)
					{
						if (result.StatusCode.IsPermanentRedirect())
							_RedirectCache.AddOrUpdateRedirect(request.RequestUri, result.Headers.Location);

						request.RequestUri = result.Headers.Location;
						result = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
					}
					redirects++;
				}
				else
					break;
			}
			return result;
		}

		#endregion

	}
}