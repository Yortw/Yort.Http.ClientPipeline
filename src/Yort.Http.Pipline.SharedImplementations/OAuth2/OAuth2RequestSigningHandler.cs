using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.OAuth2
{
	/// <summary>
	/// A <see cref="System.Net.Http.DelegatingHandler"/> that manages access tokens and signs requests using OAuth 2.0 authentication flows.
	/// </summary>
	public class OAuth2RequestSigningHandler : System.Net.Http.DelegatingHandler
	{

		#region Fields

		private OAuth2Settings _Settings;
		private OAuth2Token _Token;
		private IRequestCondition _RequestCondition;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new instance using the specified <see cref="OAuth2Settings"/>.
		/// </summary>
		/// <param name="settings">An <see cref="OAuth2Settings"/> instance containing details used to request and manage tokens and authentication flows.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="settings"/> is null.</exception>
		public OAuth2RequestSigningHandler(OAuth2Settings settings) : this(settings, CreateDefaultInnerHandler(), null)
		{
		}

		/// <summary>
		/// Constructs a new instance using the specified <see cref="OAuth2Settings"/>.
		/// </summary>
		/// <param name="settings">An <see cref="OAuth2Settings"/> instance containing details used to request and manage tokens and authentication flows.</param>
		/// <param name="innerHandler">The inner <see cref="System.Net.Http.HttpMessageHandler"/> to call in the pipeline.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="settings"/> is null.</exception>
		public OAuth2RequestSigningHandler(OAuth2Settings settings, System.Net.Http.HttpMessageHandler innerHandler) : this(settings, innerHandler, null)
		{
		}

		/// <summary>
		/// Constructs a new instance using the specified <see cref="OAuth2Settings"/>.
		/// </summary>
		/// <param name="settings">An <see cref="OAuth2Settings"/> instance containing details used to request and manage tokens and authentication flows.</param>
		/// <param name="innerHandler">The inner <see cref="System.Net.Http.HttpMessageHandler"/> to call in the pipeline.</param>
		/// <param name="requestCondition">An optional <see cref="IRequestCondition"/> used to determine if authorisation is required. If null, then authorisation is always performed.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="settings"/> is null.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if a validation error occurs for the <paramref name="settings"/> argument. See <see cref="OAuth2Settings.Validate"/>.</exception>
		public OAuth2RequestSigningHandler(OAuth2Settings settings, System.Net.Http.HttpMessageHandler innerHandler, IRequestCondition requestCondition) : base(innerHandler)
		{
			if (settings == null) throw new ArgumentNullException(nameof(settings));
			settings.Validate();

			_RequestCondition = requestCondition;
			_Settings = settings;
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Signs/authorizes requests before passing them on to the inner handler.
		/// </summary>
		/// <param name="request">The request to be signed.</param>
		/// <param name="cancellationToken">A cancellation token used to cancel the request.</param>
		/// <returns>A task whose result is a <see cref="HttpResponseMessage"/> instance.</returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (ShouldAuthoriseRequest(request, _Settings.RequestSigningMethod))
			{
				try
				{
					var token = await AcquireToken(request).ConfigureAwait(false);
					cancellationToken.ThrowIfCancellationRequested();
					if (token == null) throw new UnauthorizedAccessException("Unable to obtain token.");

					token.SignRequest(request, _Settings.RequestSigningMethod, _Settings.TokenQueryStringKey);
				}
				catch (UnauthorizedAccessException uae)
				{
					return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
					{
						ReasonPhrase = uae.Message
					};
				}
			}

			return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}

		#endregion

		#region Public Static Members

		/// <summary>
		/// Provides an authorisation code by deserialising the body of the response from an authentication uri request as json.
		/// </summary>
		/// <remarks>
		/// <para>You almost certainly do not want to use this. A proper OAuth2 authorization_code flow should involve user interaction to authenticate. 
		/// This method avoids that. Instead it assumes the authorisation url returns a simple json object with a 'code' property containing the authorisation code.
		/// Typically this is not the case, so it will not work. However, some third party systems have implemented this style of authentication for systems integrations when
		/// they should have implemented the client or implicit grant flows instead. In those cases, this method can be provided to the <see cref="OAuth2.OAuth2Settings.RequestAuthentication"/>
		/// property to enable the auth flow without additional code.</para>
		/// </remarks>
		/// <param name="authorisationUri"></param>
		/// <returns></returns>
		public static async Task<AuthorisationCodeResponse> NonInteractiveAuthenticationByJsonResponse(Uri authorisationUri)
		{
			using (var client = CreateDefaultHttpClient())
			{
				var authCodeResult = await client.GetAsync(authorisationUri).ConfigureAwait(false);
				authCodeResult.EnsureSuccessStatusCode();
				string authCodeResponse = await authCodeResult.Content.ReadAsStringAsync().ConfigureAwait(false);

				var retVal = new AuthorisationCodeResponse();

				var jsonobject = Newtonsoft.Json.Linq.JObject.Parse(authCodeResponse);
				retVal.AuthorisationCode = jsonobject["code"].ToString();
				if (jsonobject["state"] != null)
					retVal.State = jsonobject["state"].ToString();

				return retVal;
			}
		}

		#endregion

		#region Private Members

		private static HttpMessageHandler CreateDefaultInnerHandler()
		{
			var handler = new System.Net.Http.HttpClientHandler();
			if (handler.SupportsAutomaticDecompression)
				handler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
			return handler;
		}

		private bool ShouldAuthoriseRequest(HttpRequestMessage request, OAuth2HttpRequestSigningMethod signingMethod)
		{
			return ((signingMethod == OAuth2HttpRequestSigningMethod.AuthorizationHeader && IsAuthHeaderMissing(request.Headers.Authorization)) ||
					(signingMethod == OAuth2HttpRequestSigningMethod.UrlQuery && IsAccessTokenQueryArgumentMissing(request.RequestUri)))
					&& (_RequestCondition?.ShouldProcess(request) ?? true);
		}

		private static bool IsAuthHeaderMissing(AuthenticationHeaderValue authorization)
		{
			return authorization == null ||
				(
					StringComparer.OrdinalIgnoreCase.Compare(authorization.Scheme, "Bearer") == 0
					&& String.IsNullOrWhiteSpace(authorization.Parameter)
				);
		}

		private static bool IsAccessTokenQueryArgumentMissing(Uri requestUri)
		{
			return String.IsNullOrWhiteSpace(requestUri.Query)
				|| requestUri.Query.IndexOf("access_token=", StringComparison.OrdinalIgnoreCase) < 0;
		}

		private async Task<OAuth2Token> AcquireToken(HttpRequestMessage requestMessage)
		{
			if (HaveValidToken())
				return _Token;
			else
			{
				HttpClient client = _Settings?.CreateHttpClient?.Invoke() ?? CreateDefaultHttpClient();

				if (_Token != null && !String.IsNullOrWhiteSpace(_Token.RefreshToken))
					return await RequestToken_RefreshTokenGrant(client, _Token, requestMessage);

				switch (_Settings.GrantType)
				{
					case OAuth2GrantTypes.AuthorizationCode:
						return await RequestToken_AuthorizationCodeGrant(client, requestMessage).ConfigureAwait(false);

					case OAuth2GrantTypes.ClientCredentials:
						return await RequestToken_ClientCredentialsGrant(client).ConfigureAwait(false);

					default:
						throw new NotImplementedException($"The {_Settings.GrantType} grant type is not implemented.");
				}
			}
		}

		private static HttpClient CreateDefaultHttpClient()
		{
			var handler = new System.Net.Http.HttpClientHandler();
			if (handler.SupportsAutomaticDecompression)
				handler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;

			return new HttpClient(handler);
		}

		private async Task<OAuth2Token> RequestToken_RefreshTokenGrant(HttpClient client, OAuth2Token token, HttpRequestMessage request)
		{
			using (var creds = await _Settings.ClientCredentialProvider.GetCredentials().ConfigureAwait(false))
			{
				var content = new System.Net.Http.MultipartFormDataContent();
				content.Add(new System.Net.Http.StringContent(OAuth2GrantTypes.RefreshToken), "grant_type");
				content.Add(new System.Net.Http.StringContent(token.RefreshToken), "refresh_token");
				content.Add(new System.Net.Http.StringContent(_Settings.Scope), "scope");
				var state = _Settings.State(request);
				if (state != null)
					content.Add(new System.Net.Http.StringContent(state), "state");

				var tokenResult = await client.PostAsync(_Settings.AccessTokenUrl, content).ConfigureAwait(false);

				try
				{
					return await ProcessTokenResponse(tokenResult).ConfigureAwait(false);
				}
				catch
				{
#if DEBUG
					throw;
#else
					//On an error, attempt to get a new token rather than refreshing one.
					_Token = null;
					return await AcquireToken(request);
#endif
				}
			}
		}

		private async Task<OAuth2Token> RequestToken_AuthorizationCodeGrant(HttpClient client, HttpRequestMessage request)
		{
			using (var creds = await _Settings.ClientCredentialProvider.GetCredentials().ConfigureAwait(false))
			{
				var authCodeUrlBuilder = new UriBuilder(_Settings.AuthorizeUrl);
				string state = _Settings?.State?.Invoke(request);

				authCodeUrlBuilder.Query = $"client_id={creds.Identifier}&redirect_uri={_Settings.RedirectUrl.ToString()}&response_type=code&scope={_Settings.Scope}";
				if (!String.IsNullOrEmpty(state))
					authCodeUrlBuilder.Query += "&state=" + state;

				var authCodeResult = await _Settings.RequestAuthentication(authCodeUrlBuilder.Uri).ConfigureAwait(false);
				if (authCodeResult == null || String.IsNullOrWhiteSpace(authCodeResult.AuthorisationCode))
					throw new UnauthorizedAccessException(authCodeResult.ErrorResponse ?? "No authorisation code returned.");

				if (authCodeResult.State != state)
					throw new UnauthorizedAccessException("Unexpected 'state' value returned from authentication url.");

				var content = new System.Net.Http.MultipartFormDataContent();
				content.Add(new System.Net.Http.StringContent(OAuth2GrantTypes.AuthorizationCode), "grant_type");
				content.Add(new System.Net.Http.StringContent(creds.Identifier), "client_id");
				content.Add(new System.Net.Http.StringContent(creds.Secret), "client_secret");
				content.Add(new System.Net.Http.StringContent(_Settings.RedirectUrl.ToString()), "redirect_uri");
				content.Add(new System.Net.Http.StringContent(_Settings.Scope), "scope");
				content.Add(new System.Net.Http.StringContent(authCodeResult.AuthorisationCode), "code");
				if (!String.IsNullOrEmpty(authCodeResult.State))
					content.Add(new System.Net.Http.StringContent(authCodeResult.AuthorisationCode), "state");

				var tokenResult = await client.PostAsync(_Settings.AccessTokenUrl, content).ConfigureAwait(false);
				return await ProcessTokenResponse(tokenResult).ConfigureAwait(false);
			}
		}

		private async Task<OAuth2Token> RequestToken_ClientCredentialsGrant(HttpClient client)
		{
			var content = new MultipartFormDataContent();
			content.Add(new StringContent(OAuth2GrantTypes.ClientCredentials, System.Text.UTF8Encoding.UTF8), "grant_type");

			var tokenRequestMessage = new HttpRequestMessage(HttpMethod.Post, _Settings.AccessTokenUrl)
			{
				Content = content
			};

			HttpResponseMessage tokenResponse = null;
			using (var creds = await _Settings.ClientCredentialProvider.GetCredentials().ConfigureAwait(false))
			{
				var encodedCredentials = StringToBase64String(creds.Identifier + ":" + creds.Secret);
				tokenRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedCredentials);
				tokenResponse = await client.SendAsync(tokenRequestMessage).ConfigureAwait(false);
			}

			return await ProcessTokenResponse(tokenResponse).ConfigureAwait(false);
		}

		private static String StringToBase64String(string value)
		{
			string retVal = null;

			var bytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(value);
			retVal = System.Convert.ToBase64String(bytes);

			return retVal;
		}

		private async Task<OAuth2Token> ProcessTokenResponse(HttpResponseMessage tokenResponse)
		{
			if (tokenResponse.Content == null)
				tokenResponse.EnsureSuccessStatusCode();

			var responseContent = await tokenResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

			if (tokenResponse.IsSuccessStatusCode)
			{
				//Not all servers return a token type.
				Type tokenType = null;
				var tokenTypeRaw = Newtonsoft.Json.Linq.JObject.Parse(responseContent)["token_type"];
				if (tokenTypeRaw == null)
					tokenType = typeof(OAuth2Token);
				else
				{
					var tokenTypeKey = tokenTypeRaw.ToString();

					if (!_Settings.SupportedTokenTypes.ContainsKey(tokenTypeKey)) throw new NotSupportedException($"The token type {tokenTypeKey} returned by the server is not supported by this client.");
					tokenType = _Settings.SupportedTokenTypes[tokenTypeKey];
				}

				var tokenData = (OAuth2Token)JsonConvert.DeserializeObject(responseContent, tokenType);

				return (_Token = (OAuth2Token)tokenData);
			}
			else
			{
				var tokenError = JsonConvert.DeserializeObject<OAuth2TokenError>(responseContent);
				throw new OAuth2Exception(tokenError);
			}
		}

		private bool HaveValidToken()
		{
			return !String.IsNullOrWhiteSpace(_Token?.AccessToken)
				&& (_Token?.Expiry == null || _Token?.Expiry > DateTime.Now.AddMinutes(2));
		}

		#endregion

	}
}