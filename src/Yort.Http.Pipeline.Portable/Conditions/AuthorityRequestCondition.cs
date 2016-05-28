using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// A <see cref="IRequestCondition"/> condtion which is true if the request's authority matches any one of a list of authorities.
	/// </summary>
	/// <remarks>
	/// <para>Authority names are treated as case sensitive.</para>
	/// </remarks>
	public class AuthorityHttpRequestCondition : IRequestCondition
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public AuthorityHttpRequestCondition()
		{
			Helper.Throw();
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="allowedAuthorities">An enumerable of authority names that are allowed by this condition.</param>
		public AuthorityHttpRequestCondition(IEnumerable<string> allowedAuthorities) : this()
		{
			Helper.Throw();
		}

		/// <summary>
		/// Returns true if the authority for the request is in the list of allowed authorities.
		/// </summary>
		/// <param name="requestMessage">The request to analyse.</param>
		/// <returns>True if the authority is allowed by this condition.</returns>
		public bool ShouldProcess(HttpRequestMessage requestMessage)
		{
			Helper.Throw();
			return false;
		}

		/// <summary>
		/// Adds an authority to the list of allowed authorities.
		/// </summary>
		/// <param name="authority">A string containing the name of the authority to add.</param>
		public void AddAuthority(string authority)
		{
			Helper.Throw();
		}

		/// <summary>
		/// Removes an authority from the list of allowed authorities.
		/// </summary>
		/// <param name="authority"></param>
		public void RemoveAuthority(string authority)
		{
			Helper.Throw();
		}
	}
}