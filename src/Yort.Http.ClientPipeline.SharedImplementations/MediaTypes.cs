using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Provides a list of common HTTP media/mime types.
	/// </summary>
	public static class MediaTypes
	{
		/// <summary>
		/// Returns "application/json".
		/// </summary>
		public const string ApplicationJson = "application/json";
		/// <summary>
		/// Returns "application/xml".
		/// </summary>
		public const string ApplicationXml = "application/xml";
		/// <summary>
		/// Returns "multipart/form-data".
		/// </summary>
		public const string MultipartFormData = "multipart/form-data";
		/// <summary>
		/// Returns "text/html".
		/// </summary>
		public const string TextHtml = "text/html";
		/// <summary>
		/// Returns "text/plain".
		/// </summary>
		public const string TextPlain = "text/plain";
		/// <summary>
		/// Returns "application/x-www-form-urlencoded".
		/// </summary>
		public const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";
	}
}