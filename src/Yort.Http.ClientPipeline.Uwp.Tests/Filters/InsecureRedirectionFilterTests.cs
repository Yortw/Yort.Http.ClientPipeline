using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;
using Yort.Http.ClientPipeline;

namespace Yort.Http.ClientPipeline.Uwp.Tests
{
	[TestClass]
	public class InsecureRedirectionFilterTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionFilter))]
		[TestCategory("MessageHandlers")]
		public void InsecureRedirectionFilter_Constructor_ThrowsOnNullInnerHandler()
		{
			Assert.ThrowsException<ArgumentNullException>(() =>
			{
				var handler = new InsecureRedirectionFilter(null);
			});
		}

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionFilter))]
		[TestCategory("MessageHandlers")]
		public void InsecureRedirectionFilter_Constructor_ConstructsOkWithZeroMaxRedirects()
		{
			var handler = new InsecureRedirectionFilter(new Windows.Web.Http.Filters.HttpBaseProtocolFilter { AllowAutoRedirect = false }, 0);
		}

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionFilter))]
		[TestCategory("MessageHandlers")]
		public void InsecureRedirectionFilter_Constructor_ThrowsOnNegativeMaxRedirects()
		{
			Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
			{
				var handler = new InsecureRedirectionFilter(new Windows.Web.Http.Filters.HttpBaseProtocolFilter { AllowAutoRedirect = false }, -1);
			});
		}

		[TestMethod]
		[TestCategory(nameof(InsecureRedirectionFilter))]
		[TestCategory("MessageHandlers")]
		public void InsecureRedirectionFilter_Constructor_ConstructsOkWithNullRedirectCache()
		{
			var handler = new InsecureRedirectionFilter(new Windows.Web.Http.Filters.HttpBaseProtocolFilter { AllowAutoRedirect = false }, 10, null);
		}

		#endregion

		[TestMethod]
		[TestCategory("Filters")]
		[TestCategory(nameof(InsecureRedirectionFilter))]
		public async Task InsecureRedirectionFilter_FollowsRedirects()
		{
			var requestUri = new Uri("https://t.co/YJ9y1xD2be");

			var handler = new InsecureRedirectionFilter(new Windows.Web.Http.Filters.HttpBaseProtocolFilter() { AllowAutoRedirect = false });
			var client = new Windows.Web.Http.HttpClient(handler);
			var result = await client.GetAsync(requestUri);

			Assert.AreEqual(Windows.Web.Http.HttpStatusCode.Ok, result.StatusCode);
			Assert.AreEqual("http://www.abc.net.au/news/2016-03-05/scott-kelly-spaceflight-leaves-astronaut-feeling-sore/7223312", result.RequestMessage.RequestUri.ToString());
		}
	}
}