using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yort.Http.ClientPipeline;
using Yort.Http.ClientPipeline.OAuth2;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class OAuth2RequestSigningHandlerTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(OAuth2RequestSigningHandler))]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void OAuth2RequestSigningHandler_Constructor_ThrowsOnNullSettings()
		{
			var handler = new OAuth2RequestSigningHandler(null);
		}

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(OAuth2RequestSigningHandler))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2RequestSigningHandler_Constructor_ThrowsOnInvalidSettings()
		{
			var settings = new OAuth2Settings();
			var handler = new OAuth2RequestSigningHandler(settings);
		}

		#endregion

#if RunTestsWithExternalDependencies
		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(OAuth2RequestSigningHandler))]
		[TestCategory("TestsWithExternaDependencies")]
		public async Task OAuth2RequestSigningHandler_CanSignRequestsUsingNonInteractiveJsonResponse()
		{
			var credentials = new SimpleCredentials()
			{
				Identifier = TestSecrets.OAuth2CredentialId,
				Secret = TestSecrets.OAuth2CredentialSecret
			};

			var settings = new OAuth2.OAuth2Settings()
			{
				AccessTokenUrl = new Uri(TestSecrets.OAuth2AccessTokenUrl),
				AuthorizeUrl = new Uri(TestSecrets.OAuth2AuthorisationUrl),
				RedirectUrl = new Uri(TestSecrets.OAuth2RedirectUrl),
				ClientCredentialProvider = new SimpleCredentialProvider(credentials),
				Scope = "master_all",
				GrantType = OAuth2.OAuth2GrantTypes.AuthorizationCode,
				RequestSigningMethod = OAuth2HttpRequestSigningMethod.UrlQuery,
				RequestAuthentication = OAuth2RequestSigningHandler.NonInteractiveAuthenticationByJsonResponse
			};

			var signer = new OAuth2RequestSigningHandler(settings);
			var client = new System.Net.Http.HttpClient(signer);
			var result = await client.GetAsync(TestSecrets.OAuth2TestUrl1);
			result.EnsureSuccessStatusCode();
			var result2 = await client.GetAsync(TestSecrets.OAuth2TestUrl2);
			result2.EnsureSuccessStatusCode();
			var content = await result2.Content.ReadAsStringAsync();
		}
#endif

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(OAuth2RequestSigningHandler))]
		public async Task OAuth2RequestSigningHandler_RequestsTokenThenSignsRequest()
		{
			#region Test Setup

			MockMessageHandler mockHandler = SetupOAuth2MockHandler();

			var credentials = new SimpleCredentials()
			{
				Identifier = "987654321",
				Secret = "abcdefghilmnopqrstuvzabc"
			};

			var settings = new OAuth2.OAuth2Settings()
			{
				CreateHttpClient = () => new System.Net.Http.HttpClient(mockHandler),
				AccessTokenUrl = new Uri("http://testsite.com/Token"),
				AuthorizeUrl = new Uri("http://testsite.com/Authorize"),
				RedirectUrl = new Uri("http://testsite.com/AuthComplete"),
				ClientCredentialProvider = new SimpleCredentialProvider(credentials),
				Scope = "master_all",
				GrantType = OAuth2.OAuth2GrantTypes.AuthorizationCode,
				RequestSigningMethod = OAuth2HttpRequestSigningMethod.AuthorizationHeader,
				RequestAuthentication = (authuri) =>
				{
					return Task.FromResult(new AuthorisationCodeResponse() { AuthorisationCode = "28770506516186843330" });
				},
				TokenQueryStringKey = "oauth_token"
			};

			var signer = new OAuth2RequestSigningHandler(settings, mockHandler);

			#endregion

			var client = new System.Net.Http.HttpClient(signer);
			var result = await client.GetAsync("http://testsite.com/TestEndpoint");
			result.EnsureSuccessStatusCode();
			Assert.AreEqual("Yay! You're authed.", await result.Content.ReadAsStringAsync());
		}

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(OAuth2RequestSigningHandler))]
		public async Task OAuth2RequestSigningHandler_UsesInitialTokenIfNotExpired()
		{
			#region Test Setup

			MockMessageHandler mockHandler = SetupOAuth2MockHandler();

			var credentials = new SimpleCredentials()
			{
				Identifier = "987654321",
				Secret = "abcdefghilmnopqrstuvzabc"
			};

			var settings = new OAuth2.OAuth2Settings()
			{
				AccessToken = new OAuth2.OAuth2Token() { AccessToken = "123", ExpiresIn = 3600, RefreshToken = "456", TokenType = "Bearer", Created = DateTime.Now },
				CreateHttpClient = () => new System.Net.Http.HttpClient(mockHandler),
				AccessTokenUrl = new Uri("http://testsite.com/Token"),
				AuthorizeUrl = new Uri("http://testsite.com/Authorize"),
				RedirectUrl = new Uri("http://testsite.com/AuthComplete"),
				ClientCredentialProvider = new SimpleCredentialProvider(credentials),
				Scope = "master_all",
				GrantType = OAuth2.OAuth2GrantTypes.AuthorizationCode,
				RequestSigningMethod = OAuth2HttpRequestSigningMethod.AuthorizationHeader,
				RequestAuthentication = (authuri) =>
				{
					throw new InvalidOperationException("New token was being requested! Existing token should have been used.");
				},
				TokenQueryStringKey = "oauth_token"
			};

			var signer = new OAuth2RequestSigningHandler(settings, mockHandler);

			#endregion

			var client = new System.Net.Http.HttpClient(signer);
			var result = await client.GetAsync("http://testsite.com/TestEndpoint");
			result.EnsureSuccessStatusCode();
			Assert.AreEqual("Yay! You're authed.", await result.Content.ReadAsStringAsync());
		}

		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(OAuth2RequestSigningHandler))]
		public async Task OAuth2RequestSigningHandler_ClientCredentialsGrant_AuthorisesOk()
		{
			#region Test Setup

			MockMessageHandler mockHandler = SetupOAuth2MockHandler();

			var credentials = new SimpleCredentials()
			{
				Identifier = "987654321",
				Secret = "abcdefghilmnopqrstuvzabc"
			};

			var settings = new OAuth2Settings();
			//Set the grant type
			settings.GrantType = OAuth2GrantTypes.ClientCredentials;
			//Set the credentials to use when requesting a new token
			settings.ClientCredentialProvider = new SimpleCredentialProvider(credentials);

			// Make sure token requests use our mock handler
			settings.CreateHttpClient = () => new System.Net.Http.HttpClient(mockHandler);

			//These settings are provided for all auth flows
			settings.AccessTokenUrl = new Uri("http://testsite.com/Token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/Authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");

			// Create a request signer using the config
			var signer = new OAuth2RequestSigningHandler(settings, mockHandler);

			#endregion

			var client = new System.Net.Http.HttpClient(signer);

			var result = await client.GetAsync("http://testsite.com/TestEndpoint");
			result.EnsureSuccessStatusCode();
		}

		private static MockMessageHandler SetupOAuth2MockHandler()
		{
			var mockHandler = new MockMessageHandler();
			mockHandler.AddFixedResponse(new Uri("http://testsite.com/Token"), new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new System.Net.Http.StringContent(String.Empty, System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson) });
			var mockResponse = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Redirect) { Content = new System.Net.Http.StringContent("Normally this is a web page the user logs into, but this test is automated and skips that.", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson) };
			mockResponse.Headers.Location = new Uri("http://testsite.com/AuthComplete?code=28770506516186843330");
			mockHandler.AddFixedResponse(new Uri("http://testsite.com/Authorize"), mockResponse);

			var tokenIssuingHandler = new MockResponseHandler();
			tokenIssuingHandler.CanHandleRequest = (request) => { return request.Method.Method == "POST" && request.RequestUri.ToString() == "http://testsite.com/Token"; };
			tokenIssuingHandler.HandleRequest = async (request) =>
			{
				var content = request.Content as System.Net.Http.FormUrlEncodedContent;
				if (content == null) return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

				var contentParams = new Dictionary<string, string>();
				var contentStr = await content.ReadAsStringAsync().ConfigureAwait(false);
				foreach (var kvp in contentStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
				{
					var parts = kvp.Split(new char[] { '=' });
					var key = Uri.UnescapeDataString(parts[0]);
					string value = null;
					if (parts.Length > 1)
						value = Uri.UnescapeDataString(parts[1]);

					contentParams.Add(key, value);
				}

				var grantType = (from c in contentParams where c.Key == "grant_type" select c.Value).FirstOrDefault();

				if (grantType == OAuth2.OAuth2GrantTypes.AuthorizationCode)
				{
					var code = (from c in contentParams where c.Key == "code" select c.Value).FirstOrDefault();

					if (code != "28770506516186843330")
						return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

					return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new System.Net.Http.StringContent("{ token_type: \"Bearer\", access_token: \"123\", expires_in: \"3600\", refresh_token: \"456\" }", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson) };
				}
				else if (grantType == OAuth2GrantTypes.ClientCredentials)
				{
					if (request.Headers.Authorization.Scheme == "Basic" && request.Headers.Authorization.Parameter == "OTg3NjU0MzIxOmFiY2RlZmdoaWxtbm9wcXJzdHV2emFiYw==")
					{
						return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new System.Net.Http.StringContent("{ token_type: \"Bearer\", access_token: \"123\", expires_in: \"3600\", refresh_token: \"456\" }", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson) };
					}
				}

				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
			};
			mockHandler.AddDynamicResponse(tokenIssuingHandler);

			var authCheckingHandler = new MockResponseHandler();
			authCheckingHandler.CanHandleRequest = (request) => { return request.RequestUri.ToString() == "http://testsite.com/TestEndpoint"; };
			authCheckingHandler.HandleRequest = (request) =>
			{
				if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer" || request.Headers.Authorization.Parameter != "123")
					return Task.FromResult(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));

				return Task.FromResult(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new System.Net.Http.StringContent("Yay! You're authed.") });
			};
			mockHandler.AddDynamicResponse(authCheckingHandler);
			return mockHandler;
		}
	}
}