using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class CompressedRequestHandlerTests
	{
		[TestMethod]
		[TestCategory(nameof(CompressedRequestHandler))]
		[TestCategory("MessageHandlers")]
		public async Task CompressedRequestHandler_PostAsync_CompressesContent()
		{
			var requestUriString = "http://sometestdomain.com/someendpoint";
			
			var mh = new MockMessageHandler();
			mh.AddFixedResponse("POST", new Uri(requestUriString), new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Created));

			var handler = new CompressedRequestHandler(mh);
			var client = new System.Net.Http.HttpClient(handler);

			var uncompressedContent = "A compressible string. A compressible string. A compressible string. A compressible string.";
			var result = await client.PostAsync(requestUriString, new System.Net.Http.StringContent(uncompressedContent)).ConfigureAwait(false);

			var modifiedContent = await mh.Requests.Last().Content.ReadAsStringAsync().ConfigureAwait(false);
			Assert.IsTrue(modifiedContent.Length < uncompressedContent.Length);

			var stream = await mh.Requests.Last().Content.ReadAsStreamAsync().ConfigureAwait(false);
			var decompressStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
			var reader = new System.IO.StreamReader(decompressStream);
			var decompressedContent = reader.ReadToEnd();

			Assert.AreEqual(uncompressedContent, decompressedContent);
		}

		[TestMethod]
		[TestCategory(nameof(CompressedRequestHandler))]
		[TestCategory("MessageHandlers")]
		public async Task CompressedRequestHandler_PostAsync_SetsContentEncodingHeader()
		{
			var requestUriString = "http://sometestdomain.com/someendpoint";

			var mh = new MockMessageHandler();
			mh.AddFixedResponse("POST", new Uri(requestUriString), new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Created));

			var handler = new CompressedRequestHandler(mh);
			var client = new System.Net.Http.HttpClient(handler);

			var uncompressedContent = "A compressible string. A compressible string. A compressible string. A compressible string.";
			var result = await client.PostAsync(requestUriString, new System.Net.Http.StringContent(uncompressedContent)).ConfigureAwait(false);

			Assert.AreEqual("gzip", mh.Requests.Last().Content.Headers.ContentEncoding.First());
		}

		[TestMethod]
		[TestCategory(nameof(CompressedRequestHandler))]
		public async Task CompressedRequestHandler_PostAsync_WorksWithNullContent()
		{
			var requestUriString = "http://sometestdomain.com/someendpoint";

			var mh = new MockMessageHandler();
			mh.AddFixedResponse("POST", new Uri(requestUriString), new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Created));

			var handler = new CompressedRequestHandler(mh);
			var client = new System.Net.Http.HttpClient(handler);

			var result = await client.PostAsync(requestUriString, null).ConfigureAwait(false);
			result.EnsureSuccessStatusCode();
		}

		[TestMethod]
		[TestCategory(nameof(CompressedRequestHandler))]
		[TestCategory("MessageHandlers")]
		public async Task CompressedRequestHandler_PutAsync_CompressesContent()
		{
			var requestUriString = "http://sometestdomain.com/someendpoint";

			var mh = new MockMessageHandler();
			mh.AddFixedResponse("PUT", new Uri(requestUriString), new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Created));

			var handler = new CompressedRequestHandler(mh);
			var client = new System.Net.Http.HttpClient(handler);

			var uncompressedContent = "A compressible string. A compressible string. A compressible string. A compressible string.";
			var result = await client.PutAsync(requestUriString, new System.Net.Http.StringContent(uncompressedContent)).ConfigureAwait(false);

			var modifiedContent = await mh.Requests.Last().Content.ReadAsStringAsync().ConfigureAwait(false);
			Assert.IsTrue(modifiedContent.Length < uncompressedContent.Length);

			var stream = await mh.Requests.Last().Content.ReadAsStreamAsync().ConfigureAwait(false);
			var decompressStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
			var reader = new System.IO.StreamReader(decompressStream);
			var decompressedContent = reader.ReadToEnd();

			Assert.AreEqual(uncompressedContent, decompressedContent);
		}

		[TestMethod]
		[TestCategory(nameof(CompressedRequestHandler))]
		[TestCategory("MessageHandlers")]
		public async Task CompressedRequestHandler_PutAsync_SetsContentEncodingHeader()
		{
			var requestUriString = "http://sometestdomain.com/someendpoint";

			var mh = new MockMessageHandler();
			mh.AddFixedResponse("PUT", new Uri(requestUriString), new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Created));

			var handler = new CompressedRequestHandler(mh);
			var client = new System.Net.Http.HttpClient(handler);

			var uncompressedContent = "A compressible string. A compressible string. A compressible string. A compressible string.";
			var result = await client.PutAsync(requestUriString, new System.Net.Http.StringContent(uncompressedContent)).ConfigureAwait(false);

			Assert.AreEqual("gzip", mh.Requests.Last().Content.Headers.ContentEncoding.First());
		}

		[TestMethod]
		[TestCategory(nameof(CompressedRequestHandler))]
		[TestCategory("MessageHandlers")]
		public async Task CompressedRequestHandler_PutAsync_WorksWithNullContent()
		{
			var requestUriString = "http://sometestdomain.com/someendpoint";

			var mh = new MockMessageHandler();
			mh.AddFixedResponse("PUT", new Uri(requestUriString), new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Created));

			var handler = new CompressedRequestHandler(mh);
			var client = new System.Net.Http.HttpClient(handler);

			var result = await client.PutAsync(requestUriString, null).ConfigureAwait(false);
			result.EnsureSuccessStatusCode();
		}


	}
}