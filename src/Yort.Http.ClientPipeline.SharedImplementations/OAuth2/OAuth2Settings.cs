using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.OAuth2
{
	/// <summary>
	/// Provides details of the endpoints and settings neccesary to authentication with an OAuth 2.0 protected service.
	/// </summary>
	public class OAuth2Settings
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public OAuth2Settings()
		{
			this.SupportedTokenTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
			this.SupportedTokenTypes.Add("Bearer", typeof(OAuth2Token));
			this.SupportedTokenTypes.Add("Token", typeof(OAuth2Token));
			this.TokenQueryStringKey = "access_token";
		}

		/// <summary>
		/// Sets or returns an existing <see cref="OAuth2Token"/> instance that will be used to sign requests until it expires, at which point a new token will be requested.
		/// </summary>
		/// <remarks>
		/// <para>If you have long lived tokens you are persisting and reusing between app sessions, set this to the deserialised token previously acquired. It will be used until it expires, and then a new token will be requested automatically.</para>
		/// </remarks>
		public OAuth2Token AccessToken { get; set; }

		/// <summary>
		/// The end point accessed to retrieve an authorisation code. Only required by authentication flows that use authorisation codes.
		/// </summary>
		public Uri AuthorizeUrl { get; set; }
		/// <summary>
		/// The end point accessed to retrieve or refresh an access token.
		/// </summary>
		public Uri AccessTokenUrl { get; set; }
		/// <summary>
		/// The redirect url to be provided to the service during authentication flows.
		/// </summary>
		public Uri RedirectUrl { get; set; }

		/// <summary>
		/// A reference to an <see cref="ICredentialProvider"/> implementation that is used to retreive the client id and secret when neccesary.
		/// </summary>
		public ICredentialProvider ClientCredentialProvider { get; set; }

		/// <summary>
		/// The type of grant being requested. The default value is <see cref="OAuth2GrantTypes.AuthorizationCode"/>.
		/// </summary>
		/// <remarks>
		/// <para>The exact value depends on the authentication flow being used, but common types are provided by <see cref="OAuth2GrantTypes"/>.</para>
		/// </remarks>
		public string GrantType { get; set; } = OAuth2GrantTypes.AuthorizationCode;

		/// <summary>
		/// The scope of access requested. This is typically equivalent to a set of 'permissions' requested. These are service specific, so check with the documentation for the service you are connecting to.
		/// </summary>
		public string Scope { get; set; }

		/// <summary>
		/// A function that returns an opaque value used by the client to maintain state between the request and callback. The authorization server includes this value when redirecting the user-agent back to the client.
		/// </summary>
		public Func<System.Net.Http.HttpRequestMessage, string> State { get; set; }

		/// <summary>
		/// An optional factory used to create <see cref="System.Net.Http.HttpClient"/> instances used for conducting OAuth 2 related HTTP calls. If null, the library will create it's own instance internally.
		/// </summary>
		/// <returns></returns>
		public Func<HttpClient> CreateHttpClient { get; set; }

		/// <summary>
		/// Sets or returns a value indicating how requests to the API are signed. The default and recommended value is <see cref="OAuth2HttpRequestSigningMethod.AuthorizationHeader"/>.
		/// </summary>
		/// <remarks>
		/// <para>Change this setting to a non-default value only if *required* by the OAuth 2.0 API.</para>
		/// </remarks>
		public OAuth2HttpRequestSigningMethod RequestSigningMethod { get; set; }

		/// <summary>
		/// Only used if <see cref="RequestSigningMethod"/> is <see cref="OAuth2HttpRequestSigningMethod.UrlQuery"/>. Specifies the key to use in the url query string for the access token.
		/// </summary>
		/// <remarks>
		/// <para>The default value is "access_token". Some servers may use "oauth_token", "token" or other names.</para>
		/// </remarks>
		public string TokenQueryStringKey { get; set; }

		/// <summary>
		/// Provides a dictionary of OAuth .Net types that represent token types this client supports.
		/// </summary>
		/// <remarks>
		/// <para>The keys for this collection are NOT case-sensitve.</para>
		/// <para>The type specified for each entry MUST derive from <see cref="OAuth2Token"/> or an invalid cast exception will occur during use.</para>
		/// </remarks>
		public IDictionary<string, Type> SupportedTokenTypes { get; private set; }

		/// <summary>
		/// A function called when the <see cref="GrantType"/> property is <see cref="OAuth2.OAuth2GrantTypes.AuthorizationCode"/> and the user is required to authenticate.
		/// </summary>
		/// <remarks>
		/// <para>The authorisation flow usually requires authentication via a 'user agent', typically a browser. This function is called with the pre-calculated authorisation uri and is expected to return a <see cref="AuthorisationCodeResponse"/> instance containing the values from the succcessful authorisation.</para>
		/// <para>Typically that process would involve using the web authentication broken or another web browser, navigated to the the authorisation url, to the user. The user authenticates using the browser which then redirects to <see cref="RedirectUrl"/> with query arguments for the authorisation code and state if any.</para>
		/// </remarks>
		public RequestAuthenticationFunction RequestAuthentication { get; set; }

		/// <summary>
		/// Checks the current setttings and throws an <see cref="InvalidOperationException"/> with a descriptive error message if any problems are found.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ClientCredentialProvider")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RedirectUrl")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AccessTokenUrl")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AuthorizeUrl")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RequestAuthentication")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AuthorizationCode")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ClientCredentialProvider")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RedirectUrl")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AccessTokenUrl")]
		public void Validate()
		{
			if (this.AccessTokenUrl == null) throw new InvalidOperationException(nameof(AccessTokenUrl) + " cannot be null.");
			if (this.ClientCredentialProvider == null) throw new InvalidOperationException("A " + nameof(ClientCredentialProvider) + " is required for the client id and secret.");

			switch (this.GrantType)
			{
				case OAuth2GrantTypes.AuthorizationCode:
					if (this.AuthorizeUrl == null) throw new InvalidOperationException(nameof(AuthorizeUrl) + " cannot be null.");
					if (this.RedirectUrl == null) throw new InvalidOperationException(nameof(RedirectUrl) + " cannot be null.");
					if (this.RequestAuthentication == null) throw new InvalidOperationException(nameof(RequestAuthentication) + " callback cannot be null for the AuthorizationCode grant type.");
					break;
				case OAuth2GrantTypes.ClientCredentials:
					break;

				case OAuth2GrantTypes.Password:
					throw new NotImplementedException("The Password grant type is not currently implemented. Pull requests appreciated!");

				case OAuth2GrantTypes.RefreshToken:
					throw new InvalidOperationException("Refresh token should not be used directly as a grant type.");

				default:
					throw new InvalidOperationException("Unknown grant type: " + this.GrantType);
			}
		}
	}

	/// <summary>
	/// A delegate for the <see cref="OAuth2.OAuth2Settings.RequestAuthentication"/> method.
	/// </summary>
	/// <param name="authorisationUri">The pre-built authorisation url to redirect the user to so they can authenticate.</param>
	/// <returns>A <see cref="AuthorisationCodeResponse"/> containing the result of the authorisation.</returns>
	public delegate Task<OAuth2.AuthorisationCodeResponse> RequestAuthenticationFunction(Uri authorisationUri);
}