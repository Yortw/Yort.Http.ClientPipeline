using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.OAuth2
{
	/// <summary>
	/// A <see cref="System.Net.Http.DelegatingHandler"/> that manages access tokens and signs requests using OAuth 2.0 authentication flows.
	/// </summary>
	public class OAuth2RequestSigningHandler : System.Net.Http.DelegatingHandler
	{

		#region Constructors

		/// <summary>
		/// Constructs a new instance using the specified <see cref="OAuth2Settings"/>.
		/// </summary>
		/// <param name="settings">An <see cref="OAuth2Settings"/> instance containing details used to request and manage tokens and authentication flows.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="settings"/> is null.</exception>
		public OAuth2RequestSigningHandler(OAuth2Settings settings) : this(settings, null, null)
		{
			Helper.Throw();
		}

		/// <summary>
		/// Constructs a new instance using the specified <see cref="OAuth2Settings"/>.
		/// </summary>
		/// <param name="settings">An <see cref="OAuth2Settings"/> instance containing details used to request and manage tokens and authentication flows.</param>
		/// <param name="innerHandler">The inner <see cref="System.Net.Http.HttpMessageHandler"/> to call in the pipeline.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="settings"/> is null.</exception>
		public OAuth2RequestSigningHandler(OAuth2Settings settings, System.Net.Http.HttpMessageHandler innerHandler) : this(settings, innerHandler, null)
		{
			Helper.Throw();
		}

		/// <summary>
		/// Constructs a new instance using the specified <see cref="OAuth2Settings"/>.
		/// </summary>
		/// <param name="settings">An <see cref="OAuth2Settings"/> instance containing details used to request and manage tokens and authentication flows.</param>
		/// <param name="innerHandler">The inner <see cref="System.Net.Http.HttpMessageHandler"/> to call in the pipeline.</param>
		/// <param name="requestCondition">An optional <see cref="IRequestCondition"/> used to determine if authorisation is required. If null, then authorisation is always performed.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="settings"/> is null.</exception>
		public OAuth2RequestSigningHandler(OAuth2Settings settings, System.Net.Http.HttpMessageHandler innerHandler, IRequestCondition requestCondition) : base(innerHandler)
		{
			Helper.Throw();
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Signs/authorizes requests before passing them on to the inner handler.
		/// </summary>
		/// <param name="request">The request to be signed.</param>
		/// <param name="cancellationToken">A cancellation token used to cancel the request.</param>
		/// <returns>A task whose result is a <see cref="HttpResponseMessage"/> instance.</returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
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
		public static Task<AuthorisationCodeResponse> NonInteractiveAuthenticationByJsonResponse(Uri authorisationUri)
		{
			Helper.Throw();
			return null;
		}

		#endregion

	}
}