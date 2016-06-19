using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.ClientPipeline.OAuth2
{
	/// <summary>
	/// Provides a list of known error responses from OAuth 2.0 authorisation servers. See https://tools.ietf.org/html/rfc6749#section-4.1.2.1 for more details.
	/// </summary>
	public static class OAuth2Error
	{
		/// <summary>
		/// Indicates the request from the client was invalid or incomplete.
		/// </summary>
		/// <remarks>
		/// <para>The request is missing a required parameter, includes an invalid parameter value, includes a parameter more than once, or is otherwise malformed.</para>
		/// </remarks>
		public const string InvalidRequest = "invalid_request";

		/// <summary>
		/// The client is not registered with the API or is using an authorization method it is not registered for.
		/// </summary>
		/// <remarks>
		/// <para>The client is not authorized to request an authorization code using this method.</para>
		/// </remarks>
		public const string UnauthorizedRequest = "unauthorized_client";

		/// <summary>
		/// The resource owner or authorization server denied the request.
		/// </summary>
		public const string AccessDenied = "access_denied";

		/// <summary>
		/// The authorization server does not support obtaining an authorization code using the requested method.
		/// </summary>
		public const string UnsupportedResponseType = "unsupported_response_type";

		/// <summary>
		/// The requested scope is invalid, unknown, or malformed.
		/// </summary>
		public const string InvalidScope = "invalid_scope";

		/// <summary>
		/// The authorization server encountered an unexpected  condition that prevented it from fulfilling the request.
		/// </summary>
		/// <remarks>
		/// <para>This response is the equivalent of an HTTP 500 internal server error.</para>
		/// </remarks>
		public const string ServerError = "server_error";

		/// <summary>
		/// The authorization server is currently unable to handle the request due to a temporary overloading or maintenance of the server.  
		/// </summary>
		/// <remarks>
		/// <para>This response is the equivalent of an HTTP 503 Service Unavailable error.</para>
		/// </remarks>
		public const string TemporarilyUnavailable = "temporarily_unavailable";
				
	}
}