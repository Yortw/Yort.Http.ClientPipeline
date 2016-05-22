using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Represents a condition or set of conditions about a <see cref="Windows.Web.Http.HttpRequestMessage" /> request.
	/// </summary>
	public interface IWebHttpRequestCondition
	{
		/// <summary>
		/// Returns a boolean indicating if the specified <paramref name="requestMessage"/> should be modified/processed/handled.
		/// </summary>
		/// <param name="requestMessage">The <see cref="Windows.Web.Http.HttpRequestMessage"/> to analyse.</param>
		/// <returns>True if the request should be handled by calling code, otherwise false.</returns>
		bool ShouldProcess(Windows.Web.Http.HttpRequestMessage requestMessage);
	}
}