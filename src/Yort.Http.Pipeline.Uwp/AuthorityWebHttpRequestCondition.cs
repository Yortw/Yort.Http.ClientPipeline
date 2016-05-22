using System;
using System.Collections.Generic;
using Windows.Web.Http;
using System.Text;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// A <see cref="IRequestCondition"/> condtion which is true if the request's authority matches any one of a list of authorities.
	/// </summary>
	/// <remarks>
	/// <para>Authority names are treated as case sensitive.</para>
	/// </remarks>
	public class AuthorityWebHttpRequestCondition : IWebHttpRequestCondition
	{

		private System.Collections.Concurrent.ConcurrentDictionary<string, string> _AllowedAuthorities;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AuthorityWebHttpRequestCondition()
		{
			_AllowedAuthorities = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="allowedAuthorities">An enumerable of authority names that are allowed by this condition.</param>
		public AuthorityWebHttpRequestCondition(IEnumerable<string> allowedAuthorities) : this()
		{
			if (allowedAuthorities == null) return;

			foreach (var item in allowedAuthorities)
			{
				_AllowedAuthorities.AddOrUpdate(item, item, (key, oldValue) => item);
			}
		}

		/// <summary>
		/// Returns true if the authority for the request is in the list of allowed authorities.
		/// </summary>
		/// <param name="requestMessage">The request to analyse.</param>
		/// <returns>True if the authority is allowed by this condition.</returns>
		public bool ShouldProcess(HttpRequestMessage requestMessage)
		{
			if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

			return _AllowedAuthorities.ContainsKey(requestMessage.RequestUri.Authority);
		}

		/// <summary>
		/// Adds an authority to the list of allowed authorities.
		/// </summary>
		/// <param name="authority">A string containing the name of the authority to add.</param>
		public void AddAuthority(string authority)
		{
			_AllowedAuthorities.TryAdd(authority, authority);
		}

		/// <summary>
		/// Removes an authority from the list of allowed authorities.
		/// </summary>
		/// <param name="authority"></param>
		public void RemoveAuthority(string authority)
		{
			string unused = null;
			_AllowedAuthorities.TryRemove(authority, out unused);
		}
	}
}