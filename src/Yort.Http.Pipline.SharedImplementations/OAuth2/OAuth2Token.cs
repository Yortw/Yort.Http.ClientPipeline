using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

namespace Yort.Http.Pipeline.OAuth2
{
	/// <summary>
	/// Represents a simple/base OAuth 2.0 token (bearer style token).
	/// </summary>
	/// <remarks>
	/// <para>Other types of token, such as MAC tokens, should derive from this class and provide additional properties for the extra values required. They should also override the <see cref="SignRequest(HttpRequestMessage, OAuth2HttpRequestSigningMethod)"/> method if neccesary, to correctly sign requests made with this token.</para>
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via Json deserialisation (reflection).")]
#if __IOS__
		[Foundation.Preserve]
#endif
	public class OAuth2Token
	{

		private DateTime _Created;
		private DateTime? _Expiry;

		/// <summary>
		/// Default constructor.
		/// </summary>
#if __IOS__
			[Foundation.Preserve]
#endif
		public OAuth2Token()
		{
			_Created = DateTime.Now;
		}

		/// <summary>
		/// The access token value.
		/// </summary>
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
		/// <summary>
		/// The type of token this class represents.
		/// </summary>
		[JsonProperty("token_type")]
		public string TokenType { get; set; }
		/// <summary>
		/// The life time in seconds of this token.
		/// </summary>
		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		/// <summary>
		/// The calculated expiry date of the token.
		/// </summary>
		/// <remarks>
		/// <para>This value is based on the system clock. If the clients clock is incorrect, the calculated expiry will also be incorrect.</para>
		/// </remarks>
		public virtual DateTime? Expiry
		{
			get
			{
				if (_Expiry == null && ExpiresIn > 0)
					_Expiry = _Created.AddSeconds(ExpiresIn);

				return _Expiry;
			}
		}

		/// <summary>
		/// An optional refresh token used to renew the access token after it expires.
		/// </summary>
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		/// <summary>
		/// Sets or returns the date and time the token was created.
		/// </summary>
		/// <remarks>
		/// <para>This is automatically set to <see cref="DateTime.Now"/> by the constructor, but should be saved and reloaded along with the other properties if persisting the token between app sessions.</para>
		/// </remarks>
		public DateTime Created
		{
			get
			{
				return _Created;
			}

			set
			{
				_Created = value;
			}
		}

		/// <summary>
		/// Signs the specified <see cref="HttpRequestMessage"/> using this token.
		/// </summary>
		/// <remarks>
		/// <para>Unless overridden this method signs the request by setting providing the token value (and token type as the 'scheme' if <paramref name="signingMethod"/>  is <see cref="OAuth2HttpRequestSigningMethod.AuthorizationHeader"/>).</para>
		/// </remarks>
		/// <param name="request">The <see cref="HttpRequestMessage"/> instance to be signed.</param>
		/// <param name="signingMethod">The way the request should be signed (auth header, url query string etc).</param>
		/// <param name="tokenQueryKey">If the <paramref name="signingMethod"/> is <see cref="OAuth2HttpRequestSigningMethod.UrlQuery"/> this is the key to use for the token value in the query string.</param>
		/// <exception cref="ArgumentNullException">Thrown if the <paramref name="request"/> argument is null.</exception>
		/// <exception cref="NotSupportedException">Thrown if an unknown or unsupported <paramref name="signingMethod"/> value is provided.</exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "OAuth")]
		public virtual void SignRequest(HttpRequestMessage request, OAuth2HttpRequestSigningMethod signingMethod, string tokenQueryKey)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));

			switch (signingMethod)
			{
				case OAuth2HttpRequestSigningMethod.AuthorizationHeader:
					request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenType, AccessToken);
					break;

				case OAuth2HttpRequestSigningMethod.UrlQuery:
					request.RequestUri = new Uri(request.RequestUri.ToString() + (String.IsNullOrEmpty(request.RequestUri.Query) ? "?" : "&") + (tokenQueryKey ?? "access_token")  + "=" + Uri.EscapeDataString(this.AccessToken));
					break;

				default:
					throw new NotSupportedException("Unsupported signing method for OAuth 2.0 token.");
			}
		}
	}
}