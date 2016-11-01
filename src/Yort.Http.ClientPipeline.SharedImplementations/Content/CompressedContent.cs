using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Represents HTTP content that will be compressed.
	/// </summary>
	public class CompressedContent : HttpContent
	{
		private HttpContent originalContent;
		private string encodingType;

		private byte[] compressedData;

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="content">An existing <see cref="System.Net.Http.HttpContent"/> instance to be compressed.</param>
		/// <param name="encodingType">The type of compression to use. Supported values are 'gzip' and 'deflate'.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="content"/> or <paramref name="encodingType"/> are null.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if <paramref name="encodingType"/> is not 'gzip' or 'deflate'.</exception>
		public CompressedContent(HttpContent content, string encodingType)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}

			if (encodingType == null)
			{
				throw new ArgumentNullException("encodingType");
			}

			this.encodingType = encodingType;

			originalContent = content;
			if (String.Compare(this.encodingType, "gzip", true) != 0 && String.Compare(this.encodingType, "deflate", true) != 0)
			{
				throw new InvalidOperationException(string.Format("Encoding '{0}' is not supported. Only supports gzip or deflate encoding.", this.encodingType));
			}

			// copy the headers from the original content
			foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers.Where(x => x.Key != "Content-Length"))
			{
				this.Headers.TryAddWithoutValidation(header.Key, header.Value);
			}

			this.Headers.ContentEncoding.Add(encodingType);
			this.Headers.ContentLength = null;
		}

		/// <summary>
		/// Returns the length of the compressed data.
		/// </summary>
		/// <param name="length">An integer to provde the length in.</param>
		/// <returns>The length of the compressed data.</returns>
		protected override bool TryComputeLength(out long length)
		{
			if (compressedData == null)
				CompressDataAsync().Wait();

			length = compressedData.Length;

			return false;
		}

		/// <summary>
		/// Compresses the content before it is sent.
		/// </summary>
		/// <param name="stream">A stream containing the content to be compressed.</param>
		/// <param name="context">A <see cref="System.Net.TransportContext"/> assoicated with the request using the content.</param>
		/// <returns></returns>
		protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			if (compressedData == null)
				await CompressDataAsync().ConfigureAwait(false);

			await stream.WriteAsync(compressedData, 0, compressedData.Length).ConfigureAwait(false);
		}
		
		private async Task CompressDataAsync()
		{
			using (var stream = new System.IO.MemoryStream())
			{
				Stream compressedStream = null;

				if (String.Compare(encodingType, "gzip", true) == 0)
					compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
				else if (String.Compare(encodingType, "deflate", true) == 0)
					compressedStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);

				await originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
				{
					compressedStream?.Dispose();
				});

				compressedData = stream.ToArray();
			}
		}
	}
}