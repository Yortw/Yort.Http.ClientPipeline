﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Returns a specific <see cref="ICredentials"/> implementation instance on every invoke of <see cref="GetCredentials"/>.
	/// </summary>
	/// <remarks>
	/// <para>Because this provider returns the same credentials instance across invocations, it cannot be used with credential implementations that are properly disposable.</para>
	/// </remarks>
	public class SimpleCredentialProvider : ICredentialProvider
	{
		private ICredentials _Credentials;

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="credentials">The credentials to provide.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="credentials"/> is null.</exception>
		public SimpleCredentialProvider(ICredentials credentials)
		{
			if (credentials == null) throw new ArgumentNullException(nameof(credentials));

			_Credentials = credentials;
		}

		/// <summary>
		/// Returns the credentials provided via the constructoor.
		/// </summary>
		/// <returns>Returns the credentials provided via the constructoor.</returns>
		public Task<ICredentials> GetCredentials()
		{
			return TaskEx.FromResult<ICredentials>(_Credentials);
		}
	}
}