using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yort.Http.ClientPipeline;
using Windows.Storage.Streams;
using Windows.Storage;
using System.IO;

namespace Yort.HttpClient.Pipeline.Portable.Tests.Content
{
	[TestClass]
	public class CompressedWebContentTests
	{

		#region Constructor Tests

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedWebContent_Constructor_ThrowsOnNullContent()
		{

			Assert.ThrowsException<ArgumentNullException>(() =>
			{
				var CompressedWebContent = new CompressedWebContent(null, "gzip");
			});
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedWebContent_Constructor_ThrowsOnNullEncoding()
		{
			Assert.ThrowsException<ArgumentNullException>(() =>
			{
				var innerContent = new Windows.Web.Http.HttpStringContent("Test");
				var CompressedWebContent = new CompressedWebContent(innerContent, null);
			});
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedWebContent_Constructor_ThrowsOnUnsupportedContentType()
		{
			Assert.ThrowsException<InvalidOperationException>(() =>
			{
				var innerContent = new Windows.Web.Http.HttpStringContent("Test");
				var CompressedWebContent = new CompressedWebContent(innerContent, "123");
			});
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedWebContent_Constructor_ConstructsOkWithContentAndGzipEncoding()
		{
			var innerContent = new Windows.Web.Http.HttpStringContent("Test");
			var CompressedWebContent = new CompressedWebContent(innerContent, "gzip");
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedWebContent_Constructor_ConstructsOkWithContentAndDeflateEncoding()
		{
			var innerContent = new Windows.Web.Http.HttpStringContent("Test");
			var CompressedWebContent = new CompressedWebContent(innerContent, "deflate");
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedWebContent_Constructor_CopiesOriginalHeaders()
		{
			var innerContent = new Windows.Web.Http.HttpStringContent("Test");
			innerContent.Headers.ContentDisposition = new Windows.Web.Http.Headers.HttpContentDispositionHeaderValue("test");
			innerContent.Headers.ContentDisposition.FileName = "test";
			var CompressedWebContent = new CompressedWebContent(innerContent, "deflate");
			Assert.AreEqual("test", CompressedWebContent.Headers.ContentDisposition.FileName);
		}

		#endregion

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedWebContent_BufferAllAsync_ExecutesWithoutException()
		{
			var innerContent = new Windows.Web.Http.HttpStringContent("Jim Jimmy Jim Jim Jimney");
			var CompressedWebContent = new CompressedWebContent(innerContent, "deflate");
			await CompressedWebContent.BufferAllAsync().AsTask().ConfigureAwait(false);
			//TODO: Apart from no exception thrown, not really sure how to test this.
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedWebContent_ReadAsBufferAsync_ReturnsCompressedBytes()
		{
			var contentString = "Jim Jimmy Jim Jim Jimney";
			var innerContent = new Windows.Web.Http.HttpStringContent(contentString);
			var CompressedWebContent = new CompressedWebContent(innerContent, "deflate");
			var result = await CompressedWebContent.ReadAsBufferAsync().AsTask().ConfigureAwait(false);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length < contentString.Length);
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedWebContent_ReadAsInputStreamAsync_ReturnsCompressedStream()
		{
			var contentString = "Jim Jimmy Jim Jim Jimney";
			var innerContent = new Windows.Web.Http.HttpStringContent(contentString);
			var CompressedWebContent = new CompressedWebContent(innerContent, "deflate");
			var result = await CompressedWebContent.ReadAsInputStreamAsync().AsTask().ConfigureAwait(false);
			var stream = result.AsStreamForRead();
			Assert.IsNotNull(stream);
			var ms = new System.IO.MemoryStream();
			stream.CopyTo(ms);
			Assert.IsTrue(ms.Length < contentString.Length);
		}

		[TestCategory(nameof(CompressedWebContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedWebContent_ReadAsStringAsync_ReturnsCompressedString()
		{
			var contentString = "Jim Jimmy Jim Jim Jimney";
			var innerContent = new Windows.Web.Http.HttpStringContent(contentString);
			var CompressedWebContent = new CompressedWebContent(innerContent, "deflate");
			var result = await CompressedWebContent.ReadAsStringAsync();
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length < contentString.Length);
		}

	}
}