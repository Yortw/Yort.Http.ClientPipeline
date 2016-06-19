using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Interface for components that are able to retrieve credentials from some storage location.
	/// </summary>
	public interface ICredentialProvider
	{
		/// <summary>
		/// Returns an awaitable <see cref="Task"/> whose result is an <see cref="ICredentials"/> implementation containing the credential values.
		/// </summary>
		/// <returns>An awaitable <see cref="Task"/> whose result is an <see cref="ICredentials"/> implementation containing the credential values.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		Task<ICredentials> GetCredentials();
	}
}