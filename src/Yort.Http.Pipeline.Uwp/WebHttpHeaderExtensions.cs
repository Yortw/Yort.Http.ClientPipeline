using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Extensions relating to HTTP header management.
	/// </summary>
	public static class WebHttpHeaderExtensions
	{

		/// <summary>
		/// Copies headers from one <see cref="Windows.Web.Http.IHttpContent"/> instance to another.
		/// </summary>
		/// <param name="source">The source <see cref="Windows.Web.Http.IHttpContent"/> to copy from.</param>
		/// <param name="destination">The destination <see cref="Windows.Web.Http.IHttpContent"/> to copy to.</param>
		public static void CopyHeadersTo(this Windows.Web.Http.IHttpContent source, Windows.Web.Http.IHttpContent destination)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (destination == null) throw new ArgumentNullException(nameof(destination));

			foreach (var header in source.Headers)
			{
				if (destination.Headers.ContainsKey(header.Key))
					destination.Headers.Remove(header.Key);

				destination.Headers.Add(header.Key, header.Value);
			}
		}

		/// <summary>
		/// Copies headers from one <see cref="Windows.Web.Http.HttpResponseMessage"/> instance to another.
		/// </summary>
		/// <param name="source">The source <see cref="Windows.Web.Http.HttpResponseMessage"/> to copy from.</param>
		/// <param name="destination">The destination <see cref="Windows.Web.Http.HttpResponseMessage"/> to copy to.</param>
		public static void CopyHeadersTo(this Windows.Web.Http.Headers.HttpResponseHeaderCollection source, Windows.Web.Http.Headers.HttpResponseHeaderCollection destination)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (destination == null) throw new ArgumentNullException(nameof(destination));

			foreach (var header in source)
			{
				if (destination.ContainsKey(header.Key))
					destination.Remove(header.Key);

				destination.Add(header.Key, header.Value);
			}
		}

		/// <summary>
		/// Copies headers from one <see cref="Windows.Web.Http.HttpRequestMessage"/> instance to another.
		/// </summary>
		/// <param name="source">The source <see cref="Windows.Web.Http.HttpRequestMessage"/> to copy from.</param>
		/// <param name="destination">The destination <see cref="Windows.Web.Http.HttpRequestMessage"/> to copy to.</param>
		public static void CopyHeadersTo(this Windows.Web.Http.Headers.HttpRequestHeaderCollection source, Windows.Web.Http.Headers.HttpRequestHeaderCollection destination)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (destination == null) throw new ArgumentNullException(nameof(destination));

			foreach (var header in source)
			{
				if (destination.ContainsKey(header.Key))
					destination.Remove(header.Key);

				destination.Add(header.Key, header.Value);
			}
		}

	}
}