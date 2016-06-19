using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class OAuth2TokenTests
	{
		[TestMethod]
		public void OAuth2Token_BearerToken_SignsRequestUsingHeader()
		{
			var token = new OAuth2.OAuth2Token()
			{
				AccessToken = "123",
				ExpiresIn = 3600,
				RefreshToken = "456",
				TokenType = "Bearer"
			};

			SignByHeaderAndTestRequest(token, "Bearer", "123");
		}

		[TestMethod]
		public void OAuth2Token_Token_SignsRequestUsingHeader()
		{
			var token = new OAuth2.OAuth2Token()
			{
				AccessToken = "123",
				ExpiresIn = 3600,
				RefreshToken = "456",
				TokenType = "Token"
			};

			SignByHeaderAndTestRequest(token, "Token", "123");
		}

		[TestMethod]
		public void OAuth2Token_BearerToken_SignsRequestUsingUrl()
		{
			var token = new OAuth2.OAuth2Token()
			{
				AccessToken = "123",
				ExpiresIn = 3600,
				RefreshToken = "456",
				TokenType = "Bearer"
			};

			SignByUrlAndTestRequest(token, "Bearer", "123");
		}

		private static void SignByHeaderAndTestRequest(OAuth2.OAuth2Token token, string expectedTokenType, string expectedToken)
		{
			var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.testsite.com/testendpoint");
			token.SignRequest(request, OAuth2.OAuth2HttpRequestSigningMethod.AuthorizationHeader, null);
			Assert.IsNotNull(request.Headers.Authorization);
			Assert.AreEqual(expectedTokenType, request.Headers.Authorization.Scheme);
			Assert.AreEqual(expectedToken, request.Headers.Authorization.Parameter);
		}

		private static void SignByUrlAndTestRequest(OAuth2.OAuth2Token token, string expectedTokenType, string expectedToken)
		{
			var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.testsite.com/testendpoint");
			token.SignRequest(request, OAuth2.OAuth2HttpRequestSigningMethod.UrlQuery, "access_token");
			var queryString = request.RequestUri.Query;
			Assert.IsFalse(String.IsNullOrWhiteSpace(queryString));
			Assert.IsTrue(queryString.Contains("access_token=" + expectedToken));
		}

	}
}