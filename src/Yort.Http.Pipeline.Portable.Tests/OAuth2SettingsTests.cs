using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yort.Http.Pipeline.OAuth2;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class OAuth2SettingsTests
	{

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_RequiresAuthenticationCallbackForAuthorizationGrant()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = OAuth2GrantTypes.AuthorizationCode;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });
			settings.RequestAuthentication = null;

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_RequiresCredentialProviderForAuthorizationGrant()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = OAuth2GrantTypes.ClientCredentials;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.ClientCredentialProvider = null;

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_RequiresCredentialProviderForClientCredentialsGrant()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = OAuth2GrantTypes.ClientCredentials;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.ClientCredentialProvider = null;

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_ThrowsOnNullAccessTokenUrl()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = OAuth2GrantTypes.AuthorizationCode;
			settings.AccessTokenUrl = null;
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.RequestAuthentication = (authUri) => { return Task.FromResult(new AuthorisationCodeResponse());  };
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_ThrowsOnNullAuthorizeUrl()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = OAuth2GrantTypes.AuthorizationCode;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = null;
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.RequestAuthentication = (authUri) => { return Task.FromResult(new AuthorisationCodeResponse()); };
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_ThrowsOnNullRedirectUrl()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = OAuth2GrantTypes.AuthorizationCode;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = null;
			settings.RequestAuthentication = (authUri) => { return Task.FromResult(new AuthorisationCodeResponse()); };
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_ThrowsOnNullGrantType()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = null;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.RequestAuthentication = (authUri) => { return Task.FromResult(new AuthorisationCodeResponse()); };
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_ThrowsOnEmptyGrantType()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = String.Empty;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.RequestAuthentication = (authUri) => { return Task.FromResult(new AuthorisationCodeResponse()); };
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_ThrowsOnInvalidGrantType()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = "What's This?";
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.RequestAuthentication = (authUri) => { return Task.FromResult(new AuthorisationCodeResponse()); };
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });

			settings.Validate();
		}

		[TestMethod]
		[TestCategory(nameof(OAuth2Settings))]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OAuth2Settings_Validate_ThrowsOnRefreshTokenGrantType()
		{
			var settings = new OAuth2Settings();
			settings.GrantType = OAuth2GrantTypes.RefreshToken;
			settings.AccessTokenUrl = new Uri("http://testsite.com/access_token");
			settings.AuthorizeUrl = new Uri("http://testsite.com/authorize");
			settings.RedirectUrl = new Uri("http://testsite.com/redirect");
			settings.RequestAuthentication = (authUri) => { return Task.FromResult(new AuthorisationCodeResponse()); };
			settings.ClientCredentialProvider = new SimpleCredentialProvider(new SimpleCredentials() { Secret = "A", Identifier = "B" });

			settings.Validate();
		}

	}
}