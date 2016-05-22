using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yort.Http.Pipeline;
using Yort.Http.Pipeline.OAuth2;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class OAuth2RequestSigningHandlerTests
	{
		[TestMethod]
		[TestCategory("MessageHandlers")]
		[TestCategory(nameof(OAuth2RequestSigningHandler))]
		public async Task OAuth2RequestSigningHandler_CanSignRequests()
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
				CredentialProvider = new SimpleCredentialProvider(credentials),
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
	}
}