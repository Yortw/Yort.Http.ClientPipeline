using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.Pipeline.OAuth2
{
	/// <summary>
	/// Represents the response from an authorisation attempt via a user agent when using the <see cref="OAuth2GrantTypes.AuthorizationCode"/> flow.
	/// </summary>
	public class AuthorisationCodeResponse
	{

		private static readonly char[] UrlKeyValuePairSeparators = new char[] { '&' };
		private static readonly char[] UrlKeyValueSeparators = new char[] { '=' };

		/// <summary>
		/// The authorisation code return from the server as a result of the authentication.
		/// </summary>
		public string AuthorisationCode { get; set; }

		/// <summary>
		/// The state, if any, that was originally passed to the authentication url.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// The error message returned if the authentication failed.
		/// </summary>
		public string ErrorResponse { get; set; }

		/// <summary>
		/// Returns a new <see cref="AuthorisationCodeResponse"/> instance containing the values parsed from the provided <paramref name="queryString"/>.
		/// </summary>
		/// <param name="queryString">The query part of a url to parse.</param>
		/// <returns>A new <see cref="AuthorisationCodeResponse"/> instance containing the parsed values.</returns>
		public static AuthorisationCodeResponse FromUrlQueryString(string queryString)
		{
			if (queryString == null) throw new ArgumentNullException(nameof(queryString));

			var retVal = new AuthorisationCodeResponse();

			if (queryString.StartsWith("/")) queryString = queryString.Substring(1);

			var keyValuePairs = queryString.Split(UrlKeyValuePairSeparators);
			foreach (var kvp in keyValuePairs)
			{
				var parts = kvp.Split(UrlKeyValueSeparators);
				switch (parts[0])
				{
					case "code":
						retVal.AuthorisationCode = Uri.UnescapeDataString(parts[1]);
						break;

					case "state":
						retVal.State = Uri.UnescapeDataString(parts[1]);
						break;
				}
			}

			return retVal;
		}
	}
}