using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Represents a condition or set of conditions about a <see cref="System.Net.Http.HttpRequestMessage" /> request.
	/// </summary>
	public interface IRequestCondition
	{
		/// <summary>
		/// Returns a boolean indicating if the specified <paramref name="requestMessage"/> should be modified/processed/handled.
		/// </summary>
		/// <param name="requestMessage">The <see cref="System.Net.Http.HttpRequestMessage"/> to analyse.</param>
		/// <returns>True if the request should be handled by calling code, otherwise false.</returns>
		bool ShouldProcess(System.Net.Http.HttpRequestMessage requestMessage);
	}
}