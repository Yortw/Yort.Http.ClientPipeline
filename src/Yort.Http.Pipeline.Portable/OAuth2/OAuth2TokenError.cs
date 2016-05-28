using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.Pipeline.OAuth2
{
	/// <summary>
	/// Represents an error returned from an authorisation server while managing (requesting/refreshing etc) a token.
	/// </summary>
#if __IOS__
		[Foundation.Preserve]
#endif
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via Json deserialisation (reflection).")]
	public class OAuth2TokenError
	{
		
		/// <summary>
		/// Default constructor.
		/// </summary>
#if __IOS__
			[Foundation.Preserve]
#endif
		public OAuth2TokenError()
		{
			Helper.Throw();
		}

		/// <summary>
		/// A short name of the error. See <see cref="OAuth2Error"/> for a list of known errors.
		/// </summary>
		[JsonProperty("error")]
		public string Error { get; set; }

		/// <summary>
		/// An optional human readable description of the error. If displaying the error to a user, this is the recommended value.
		/// </summary>
		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }

		/// <summary>
		/// An optional reference to a web page containing more information about the error that occurred.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[JsonProperty("error_uri")]
		public string ErrorUri { get; set; }
	}
}