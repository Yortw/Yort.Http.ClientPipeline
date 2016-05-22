using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.Pipeline.OAuth2
{
	/// <summary>
	/// An enum specifying the ways a token or signature can be applied to a request.
	/// </summary>
	public enum OAuth2HttpRequestSigningMethod
	{
		/// <summary>
		/// The default and preferred mechansim. Values are set in the HTTP Authorization header of the request.
		/// </summary>
		AuthorizationHeader = 0,
		/// <summary>
		/// Not recommended but required by some clients or API implementations. The token or signing data is included in the query string of the request url as key/value pairs.
		/// </summary>
		UrlQuery
	}
}