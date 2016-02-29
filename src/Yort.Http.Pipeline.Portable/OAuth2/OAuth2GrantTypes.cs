using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.Pipeline.OAuth2
{
	/// <summary>
	/// Provides a list of known grant types for OAuth 2.0 authentication flows.
	/// </summary>
	public static class OAuth2GrantTypes
	{
		/// <summary>
		/// Used with the normal OAuth 2 authentication flow. Requests an authorization code which is swapped for an access token.
		/// </summary>
		/// <remarks>
		/// <para>See https://tools.ietf.org/html/rfc6749#section-4.2 for details of this authentication flow.</para>
		/// </remarks>
		public const string AuthorizationCode = "authorization_code";
		/// <summary>
		/// Used for confidential/authenticated clients, usually when no user is involved (systems integration). This authentication flow does not use an authorization code and authenticates directly to the token endpoint.
		/// </summary>
		/// <remarks>
		/// <para>See https://tools.ietf.org/html/rfc6749#section-4.4 for details of this authentication flow.</para>
		/// </remarks>
		public const string ClientCredentials = "client_credentails";
		/// <summary>
		/// Used when requesting an new access token using a previously provided refresh token.
		/// </summary>
		/// <remarks>
		/// <para>Typically this grant type is used by the library itself and this value is not needed directly by application code.</para>
		/// <para>See https://tools.ietf.org/html/rfc6749#page-47 for details of how refresh tokens work.</para>
		/// </remarks>
		public const string RefreshToken = "refresh_token";
		/// <summary>
		/// Used for the "Resource Owner Password Credentials Grant" authentication flow, where a password is used to authenticate.
		/// </summary>
		/// <remarks>
		/// <para>
		/// See https://tools.ietf.org/html/rfc6749#section-4.3 for details of this authentication flow.
		/// </para>
		/// </remarks>
		public const string Password = "password";
	}
}