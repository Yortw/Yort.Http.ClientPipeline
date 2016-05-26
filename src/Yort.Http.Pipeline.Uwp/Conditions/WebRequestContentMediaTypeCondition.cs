
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// A <see cref="IRequestCondition"/> condition which is true if the request's content type headers media type is one of an allowed set.
	/// </summary>
	/// <remarks>
	/// <para>Authority names are treated as case sensitive.</para>
	/// </remarks>
	public class WebRequestContentMediaTypeCondition : IRequestCondition
	{

		private System.Collections.Concurrent.ConcurrentDictionary<string, string> _AllowedMediaTypes;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WebRequestContentMediaTypeCondition()
		{
			_AllowedMediaTypes = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="allowedAuthorities">An enumerable of media types that are allowed by this condition.</param>
		public WebRequestContentMediaTypeCondition(IEnumerable<string> allowedAuthorities) : this()
		{
			if (allowedAuthorities == null) return;

			foreach (var item in allowedAuthorities)
			{
				_AllowedMediaTypes.AddOrUpdate(item, item, (key, oldValue) => item);
			}
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
			var contentType = requestMessage?.Content?.Headers?.ContentType.MediaType;
			if (String.IsNullOrWhiteSpace(contentType)) return false; 

			return _AllowedMediaTypes.ContainsKey(contentType);
		}

		/// <summary>
		/// Adds a content type to the list of allowed content types.
		/// </summary>
		/// <param name="mediaType">A string containing the name of the content type to add.</param>
		public void AddContentMediaType(string mediaType)
		{
			_AllowedMediaTypes.TryAdd(mediaType, mediaType);
		}

		/// <summary>
		/// Removes a content type from the list of allowed content types.
		/// </summary>
		/// <param name="mediaType"></param>
		public void RemoveContentMediaType(string mediaType)
		{
			string unused = null;
			_AllowedMediaTypes.TryRemove(mediaType, out unused);
		}
	}
}