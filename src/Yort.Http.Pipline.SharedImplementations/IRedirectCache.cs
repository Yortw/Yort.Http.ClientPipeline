using System;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// An interface for components that can cache redirected Url's to their redirected location.
	/// </summary>
	public interface IRedirectCache
	{
		/// <summary>
		/// Adds a new redirect, or updates the existing redirect if one exists, for the <paramref name="originalUri"/>.
		/// </summary>
		/// <param name="originalUri">The uri that was redirected.</param>
		/// <param name="redirectUri">The uri that was redirected to.</param>
		void AddOrUpdateRedirect(Uri originalUri, Uri redirectUri);
		/// <summary>
		/// Clears all known redirects from the cache.
		/// </summary>
		void Clear();
		/// <summary>
		/// Retrieves the final destination of a Url from the cached redirect list. Returns the original uri if no redirect is known.
		/// </summary>
		/// <param name="originalUri"></param>
		/// <returns></returns>
		Uri GetFinalUri(Uri originalUri);
		/// <summary>
		/// Removes a specific redirect from the cache.
		/// </summary>
		/// <param name="originalUri">The uri to remove the redirect for.</param>
		void RemoveRedirect(Uri originalUri);
	}
}