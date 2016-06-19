using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using System.IO;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Automatically (gzip) compresses request contents sets the content encoding header to gzip.
	/// </summary>
	public sealed class CompressedRequestFilter : IHttpFilter 
	{

		#region Fields

		private IHttpFilter _InnerFilter;
		private IWebHttpRequestCondition _RequestCondition;

		#endregion

		#region Constructors

		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the pipeline to pass requests through.</param>
		public CompressedRequestFilter(IHttpFilter innerFilter)  : this(innerFilter, null)
		{
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="innerFilter">The next handler in the pipeline to pass requests through.</param>
		/// <param name="requestCondition">A <see cref="IWebHttpRequestCondition"/> that controls whether any individual request is compressed or not. If null, all requests are compressed.</param>
		public CompressedRequestFilter(IHttpFilter innerFilter, IWebHttpRequestCondition requestCondition)
		{
			if (innerFilter == null) throw new ArgumentNullException(nameof(innerFilter));

			_InnerFilter = innerFilter;
			_RequestCondition = requestCondition;
		}

		#endregion

		#region IHttpFilter Members

		/// <summary>
		/// Compresses the content if suitable, sets the content encoding header and passes on the request to the next handler in the pipeline.
		/// </summary>
		/// <param name="request">The <see cref="Windows.Web.Http.HttpRequestMessage"/> to send.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="Windows.Web.Http.HttpResponseMessage"/> from the server.</returns>
		public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
		{
			return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
			{
				if (ShouldCompress(request))
					request.Content = await GetCompressedContent(request.Content).ConfigureAwait(false);

				return await _InnerFilter.SendRequestAsync(request).AsTask().ConfigureAwait(false);
			});
		}

		#endregion

		#region Private Methods

		private static async Task<IHttpContent> GetCompressedContent(IHttpContent originalContent)
		{
			var ms = new System.IO.MemoryStream();
			try
			{
				await CompressOriginalContentStream(originalContent, ms).ConfigureAwait(false);

				ms.Seek(0, System.IO.SeekOrigin.Begin);
				
				var compressedContent = new Windows.Web.Http.HttpStreamContent(ms.AsInputStream());
				originalContent.CopyHeadersTo(compressedContent);
				compressedContent.Headers.ContentEncoding.Clear();
				compressedContent.Headers.ContentEncoding.Add(new Windows.Web.Http.Headers.HttpContentCodingHeaderValue("gzip"));

				originalContent.Dispose();
				
				return compressedContent;
			}
			catch
			{
				ms?.Dispose();
				throw;
			}
		}

		private static async Task CompressOriginalContentStream(IHttpContent originalContent, System.IO.MemoryStream ms)
		{
			using (var compressingStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true))
			{
				using (var originalContentStream = await originalContent.ReadAsInputStreamAsync().AsTask().ConfigureAwait(false))
				{
					originalContentStream.AsStreamForRead().CopyTo(compressingStream);
				}
				compressingStream.Flush();
			}
		}

		private bool ShouldCompress(HttpRequestMessage request)
		{
			return request.Content != null
				&& !(request.Content?.Headers?.ContentEncoding?.Contains(new Windows.Web.Http.Headers.HttpContentCodingHeaderValue("gzip")) ?? false)
				&& !(request.Content?.Headers?.ContentEncoding?.Contains(new Windows.Web.Http.Headers.HttpContentCodingHeaderValue("deflate")) ?? false)
				&& (_RequestCondition?.ShouldProcess(request) ?? true); 
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		public void Dispose()
		{
			_InnerFilter?.Dispose();
		}

		#endregion

	}
}