using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yort.Http.ClientPipeline;

namespace Yort.HttpClient.Pipeline.Portable.Tests.Content
{
	[TestClass]
	public class CompressedContentTests
	{

		#region Constructor Tests

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void CompressedContent_Constructor_ThrowsOnNullContent()
		{
			var compressedContent = new CompressedContent(null, "gzip");
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void CompressedContent_Constructor_ThrowsOnNullEncoding()
		{
			var innerContent = new System.Net.Http.StringContent("Test");
			var compressedContent = new CompressedContent(innerContent, null);
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public void CompressedContent_Constructor_ThrowsOnUnsupportedContentType()
		{
			var innerContent = new System.Net.Http.StringContent("Test");
			var compressedContent = new CompressedContent(innerContent, "123");
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedContent_Constructor_ConstructsOkWithContentAndGzipEncoding()
		{
			var innerContent = new System.Net.Http.StringContent("Test");
			var compressedContent = new CompressedContent(innerContent, "gzip");
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedContent_Constructor_ConstructsOkWithContentAndDeflateEncoding()
		{
			var innerContent = new System.Net.Http.StringContent("Test");
			var compressedContent = new CompressedContent(innerContent, "deflate");
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[TestMethod]
		public void CompressedContent_Constructor_CopiesOriginalHeaders()
		{
			var innerContent = new System.Net.Http.StringContent("Test");
			innerContent.Headers.TryAddWithoutValidation("X-CustomHeader", "123");
			var compressedContent = new CompressedContent(innerContent, "deflate");
			IEnumerable<string> values;
			compressedContent.Headers.TryGetValues("X-CustomHeader", out values);
			Assert.AreEqual("123", values.First());
		}

		#endregion

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedContent_LoadIntoBufferAsync_ExecutesWithoutException()
		{
			var innerContent = new System.Net.Http.StringContent("Jim Jimmy Jim Jim Jimney");
			var compressedContent = new CompressedContent(innerContent, "deflate");
			await compressedContent.LoadIntoBufferAsync().ConfigureAwait(false);
			//TODO: Apart from no exception thrown, not really sure how to test this.
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedContent_ReadAsByteArrayAsync_ReturnsCompressedBytes()
		{
			var contentString = "Jim Jimmy Jim Jim Jimney";
			var innerContent = new System.Net.Http.StringContent(contentString);
			var compressedContent = new CompressedContent(innerContent, "deflate");
			var result = await compressedContent.ReadAsByteArrayAsync();
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length < contentString.Length);
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedContent_ReadAsStreamAsync_ReturnsCompressedStream()
		{
			var contentString = "Jim Jimmy Jim Jim Jimney";
			var innerContent = new System.Net.Http.StringContent(contentString);
			var compressedContent = new CompressedContent(innerContent, "deflate");
			var result = await compressedContent.ReadAsStreamAsync();
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length < contentString.Length);
		}

		[TestCategory(nameof(CompressedContent))]
		[TestCategory("Content")]
		[TestMethod]
		public async Task CompressedContent_ReadAsStringAsync_ReturnsCompressedString()
		{
			var contentString = "Jim Jimmy Jim Jim Jimney";
			var innerContent = new System.Net.Http.StringContent(contentString);
			var compressedContent = new CompressedContent(innerContent, "deflate");
			var result = await compressedContent.ReadAsStringAsync();
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length < contentString.Length);
		}

	}
}