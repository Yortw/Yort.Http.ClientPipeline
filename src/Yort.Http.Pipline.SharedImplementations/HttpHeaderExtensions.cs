using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Extensions relating to HTTP header management.
	/// </summary>
	public static class HttpHeaderExtensions
	{

		/// <summary>
		/// Copies headers from one <see cref="System.Net.Http.HttpContent"/> instance to another.
		/// </summary>
		/// <param name="source">The source <see cref="System.Net.Http.HttpContent"/> to copy from.</param>
		/// <param name="destination">The destination <see cref="System.Net.Http.HttpContent"/> to copy to.</param>
		public static void CopyHeadersTo(this System.Net.Http.HttpContent source, System.Net.Http.HttpContent destination)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (destination == null) throw new ArgumentNullException(nameof(destination));

			foreach (var header in source.Headers)
			{
				if (destination.Headers.Contains(header.Key))
					destination.Headers.Remove(header.Key);

				destination.Headers.Add(header.Key, header.Value);
			}
		}

		/// <summary>
		/// Copies headers from one <see cref="System.Net.Http.HttpResponseMessage"/> instance to another.
		/// </summary>
		/// <param name="source">The source <see cref="System.Net.Http.HttpResponseMessage"/> to copy from.</param>
		/// <param name="destination">The destination <see cref="System.Net.Http.HttpResponseMessage"/> to copy to.</param>
		public static void CopyHeadersTo(this System.Net.Http.Headers.HttpResponseHeaders source, System.Net.Http.Headers.HttpResponseHeaders destination)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (destination == null) throw new ArgumentNullException(nameof(destination));

			foreach (var header in source)
			{
				if (destination.Contains(header.Key))
					destination.Remove(header.Key);

				destination.Add(header.Key, header.Value);
			}
		}

		/// <summary>
		/// Copies headers from one <see cref="System.Net.Http.HttpRequestMessage"/> instance to another.
		/// </summary>
		/// <param name="source">The source <see cref="System.Net.Http.HttpRequestMessage"/> to copy from.</param>
		/// <param name="destination">The destination <see cref="System.Net.Http.HttpRequestMessage"/> to copy to.</param>
		public static void CopyHeadersTo(this System.Net.Http.Headers.HttpRequestHeaders source, System.Net.Http.Headers.HttpRequestHeaders destination)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (destination == null) throw new ArgumentNullException(nameof(destination));

			foreach (var header in source)
			{
				if (destination.Contains(header.Key))
					destination.Remove(header.Key);

				destination.Add(header.Key, header.Value);
			}
		}

	}
}