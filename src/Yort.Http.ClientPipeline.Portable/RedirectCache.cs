using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Caches redirected Url's to their redirected location.
	/// </summary>
	public class RedirectCache : IRedirectCache
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public RedirectCache()
		{
			Helper.Throw();
		}

		/// <summary>
		/// Retrieves the final destination of a Url from the cached redirect list. Returns the original uri if no redirect is known.
		/// </summary>
		/// <param name="originalUri"></param>
		/// <returns></returns>
		public Uri GetFinalUri(Uri originalUri)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Adds a new redirect, or updates the existing redirect if one exists, for the <paramref name="originalUri"/>.
		/// </summary>
		/// <param name="originalUri">The uri that was redirected.</param>
		/// <param name="redirectUri">The uri that was redirected to.</param>
		public void AddOrUpdateRedirect(Uri originalUri, Uri redirectUri)
		{
			Helper.Throw();
		}

		/// <summary>
		/// Clears all known redirects from the cache.
		/// </summary>
		public void Clear()
		{
			Helper.Throw();
		}

		/// <summary>
		/// Removes a specific redirect from the cache.
		/// </summary>
		/// <param name="originalUri">The uri to remove the redirect for.</param>
		public void RemoveRedirect(Uri originalUri)
		{
			Helper.Throw();
		}
	}
}