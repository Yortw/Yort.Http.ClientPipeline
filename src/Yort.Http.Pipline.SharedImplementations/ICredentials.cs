using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Represents a two part credential, where one part is an identifier (i.e user name) and the second part is a secret (i.e a password).
	/// </summary>
	public interface ICredentials : IDisposable
	{
		/// <summary>
		/// The user name, consumer key, access token or other identifying part of the credential.
		/// </summary>
		string Identifier { get; set; }

		/// <summary>
		/// The password, consumer secrent, access token secret or other secret part of the credential.
		/// </summary>
		string Secret { get; set; }
	}
}