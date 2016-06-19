using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Extensions for <see cref="System.Net.HttpStatusCode"/>.
	/// </summary>
	public static class HttpStatusCodeExtensions
	{
		/// <summary>
		/// Returns true if <paramref name="statusCode"/> represents any kind of redirect or moved response.
		/// </summary>
		/// <param name="statusCode">The status code to check.</param>
		/// <remarks>
		/// <para>Returns true if the <see cref="IsPermanentRedirect(HttpStatusCode)"/> returns true, or if the status is one of;</para>
		/// <list type="bullet">
		/// <item><see cref="HttpStatusCode.Moved"/></item>
		/// <item><see cref="HttpStatusCode.RedirectKeepVerb"/></item>
		/// <item><see cref="HttpStatusCode.TemporaryRedirect"/></item>
		/// </list>
		/// </remarks>
		/// <returns>True if the status code represents a redirect or moved response.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "statusCode")]
		public static bool IsRedirect(this HttpStatusCode statusCode)
		{
			Helper.Throw();
			return false;
		}

		/// <summary>
		/// Returns true if <paramref name="statusCode"/> represents any kind of permanent redirect or moved response.
		/// </summary>
		/// <param name="statusCode">The status code to check.</param>
		/// <remarks>
		/// <para>Returns true if the status is one of;</para>
		/// <list type="bullet">
		/// <item><see cref="HttpStatusCode.MovedPermanently"/></item>
		/// <item><see cref="HttpStatusCode.Redirect"/></item>
		/// </list>
		/// </remarks>
		/// <returns>True if the status code represents a permanent redirect or moved response.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "statusCode")]
		public static bool IsPermanentRedirect(this HttpStatusCode statusCode)
		{
			Helper.Throw();
			return false;
		}

	}
}