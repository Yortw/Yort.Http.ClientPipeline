using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// A <see cref="IRequestCondition"/> condition which is true if the request's content type headers media type is one of an allowed set.
	/// </summary>
	/// <remarks>
	/// <para>Authority names are treated as case sensitive.</para>
	/// </remarks>
	public class RequestContentMediaTypeCondition : IRequestCondition
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public RequestContentMediaTypeCondition()
		{
			Helper.Throw();
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="allowedAuthorities">An enumerable of media types that are allowed by this condition.</param>
		public RequestContentMediaTypeCondition(IEnumerable<string> allowedAuthorities) : this()
		{
			Helper.Throw();
		}

		/// <summary>
		/// Returns true if the authority for the request is in the list of allowed authorities.
		/// </summary>
		/// <param name="requestMessage">The request to analyse.</param>
		/// <remarks>
		/// <para>If the content or the content type header of the request is null or empty, the return value is false.</para>
		/// </remarks>
		/// <returns>True if the authority is allowed by this condition.</returns>
		public bool ShouldProcess(HttpRequestMessage requestMessage)
		{
			Helper.Throw();
			return false;
		}

		/// <summary>
		/// Adds a content type to the list of allowed content types.
		/// </summary>
		/// <param name="mediaType">A string containing the name of the content type to add.</param>
		public void AddContentMediaType(string mediaType)
		{
			Helper.Throw();
		}

		/// <summary>
		/// Removes a content type from the list of allowed content types.
		/// </summary>
		/// <param name="mediaType"></param>
		public void RemoveContentMediaType(string mediaType)
		{
			Helper.Throw();
		}
	}
}