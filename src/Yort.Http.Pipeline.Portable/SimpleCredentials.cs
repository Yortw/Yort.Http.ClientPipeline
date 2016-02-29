using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.Pipeline
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
		/// The password, consumer secrent, access token secret or other secret part of the credential.
		/// </summary>
		public string Secret
		{
			get;
			set;
		}

		/// <summary>
		/// Disposes this instance and all internal resouroces.
		/// </summary>
		public void Dispose()
		{
		}
	}
}