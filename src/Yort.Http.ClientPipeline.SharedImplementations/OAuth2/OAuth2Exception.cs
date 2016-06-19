using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.ClientPipeline.OAuth2
{
	/// <summary>
	/// An exception class for errors that occur while trying to obtain or refresh an OAuth 2.0 token.
	/// </summary>
#if SUPPORTS_SERIALISATION
	[System.Serializable]
#endif
	public class OAuth2Exception : Exception
	{
		private readonly OAuth2.OAuth2TokenError _TokenError;

		/// <summary>
		/// Default constructor required by API guideliness. Not recommended for use, see <see cref="OAuth2Exception(OAuth2TokenError)"/> for the recommended constructor.
		/// </summary>
		public OAuth2Exception() : this("An error occurred managing the OAuth 2.0 authentication.")
		{
		}

		/// <summary>
		/// Recommended constructor. Initialises an exception instance using the details provided by the <paramref name="tokenError"/> argument.
		/// </summary>
		/// <param name="tokenError">A <see cref="OAuth2TokenError"/> instance representing the error that was returned from the server.</param>
		public OAuth2Exception(OAuth2.OAuth2TokenError tokenError) : this(tokenError?.ErrorDescription ?? tokenError?.Error ?? "An error occurred managing the OAuth 2.0 authentication.")
		{
			_TokenError = tokenError;
		}

		/// <summary>
		/// Constructor that takes a custom error message but no <see cref="TokenError"/> value. Not recommended for use, see <see cref="OAuth2Exception(OAuth2TokenError)"/> for the recommended constructor.
		/// </summary>
		/// <param name="message">A custom error message to apply to the exception instance.</param>
		public OAuth2Exception(string message) : base(message) { }

		/// <summary>
		/// Constructor that takes a custom error message and inner exception but no <see cref="TokenError"/> value. Not recommended for use, see <see cref="OAuth2Exception(OAuth2TokenError)"/> for the recommended constructor.
		/// </summary>
		/// <param name="message">A custom error message to apply to the exception instance.</param>
		/// <param name="inner">The original exception being wrapped by this exception instance.</param>
		public OAuth2Exception(string message, Exception inner) : base(message, inner) { }

#if SUPPORTS_SERIALISATION
		/// <summary>
		/// Constructor used to deserialise exception.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected OAuth2Exception(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{ }
#endif

		/// <summary>
		/// Returns a <see cref="OAuth2TokenError"/> instance containing details of the error returned by the server while managing a token.
		/// </summary>
		public OAuth2TokenError TokenError
		{
			get
			{
				return _TokenError;
			}
		}

	}
}