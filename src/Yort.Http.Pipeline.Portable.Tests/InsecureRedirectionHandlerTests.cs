using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class InsecureRedirectionHandlerTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionHandler))]
		[TestCategory("MessageHandlers")]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void InsecureRedirectionHandler_Constructor_ThrowsOnNullInnerHandler()
		{
			var handler = new InsecureRedirectionHandler(null);
		}

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionHandler))]
		[TestCategory("MessageHandlers")]
		public void InsecureRedirectionHandler_Constructor_ConstructsOkWithZeroMaxRedirects()
		{
			var handler = new InsecureRedirectionHandler(new System.Net.Http.HttpClientHandler() { AllowAutoRedirect = false }, 0);
		}

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionHandler))]
		[TestCategory("MessageHandlers")]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void InsecureRedirectionHandler_Constructor_ThrowsOnNegativeMaxRedirects()
		{
			var handler = new InsecureRedirectionHandler(new System.Net.Http.HttpClientHandler() { AllowAutoRedirect = false }, -1);
		}

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionHandler))]
		[TestCategory("MessageHandlers")]
		public void InsecureRedirectionHandler_Constructor_ConstructsOkWithNullRedirectCache()
		{
			var handler = new InsecureRedirectionHandler(new System.Net.Http.HttpClientHandler() { AllowAutoRedirect = false }, 10, null);
		}

		#endregion

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionHandler))]
		[TestCategory("MessageHandlers")]
		public async Task InsecureRedirectionHandler_FollowsRedirects()
		{
			var requestUriString = "https://t.co/YJ9y1xD2be";

			var handler = new InsecureRedirectionHandler(new System.Net.Http.HttpClientHandler() { AllowAutoRedirect = false });
			var client = new System.Net.Http.HttpClient(handler);
			var result = await client.GetAsync(requestUriString);

			result = await client.GetAsync(requestUriString);

			Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
		}
	}
}