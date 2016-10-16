using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Provides a simple implementation of the <see cref="ICredentials"/> interface.
	/// </summary>
	/// <remarks>
	/// <para>This implementation holds the credentials in memory as strings. There is no encryption or protection of the string values, attacks such as RAM scraping or swap file analysis may be able to locate the raw unencrypted credentials.</para>
	/// </remarks>
	public sealed class SimpleCredentials : ICredentials
	{

		/// <summary>
		/// The user name, consumer key, access token or other identifying part of the credential.
		/// </summary>
		public string Identifier
		{
			get;
			set;
		}

		/// <summary>
		/// The password, consumer secret, access token secret or other secret part of the credential.
		/// </summary>
		public string Secret
		{
			get;
			set;
		}

		/// <summary>
		/// Does nothing but is provided for compatibility with other credential types that may need disposal.
		/// </summary>
		public void Dispose()
		{
		}
	}
}