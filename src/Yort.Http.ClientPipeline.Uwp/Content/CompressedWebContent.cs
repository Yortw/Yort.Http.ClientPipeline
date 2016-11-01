using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// Represents HTTP content that will be compressed.
	/// </summary>
	public class CompressedWebContent : IHttpContent
	{
		private IHttpContent originalContent;
		private HttpContentHeaderCollection headers;
		private string encodingType;

		private IBuffer bufferedData;

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="content">An existing <see cref="Windows.Web.Http.IHttpContent"/> implementation to be compressed.</param>
		/// <param name="encodingType">The type of compression to use. Supported values are 'gzip' and 'deflate'.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="content"/> or <paramref name="encodingType"/> are null.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if <paramref name="encodingType"/> is not 'gzip' or 'deflate'.</exception>
		public CompressedWebContent(IHttpContent content, string encodingType)
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

			headers = new HttpContentHeaderCollection();
			originalContent.CopyHeadersTo(this);

			headers.ContentEncoding.Add(new HttpContentCodingHeaderValue(encodingType));
			headers.ContentLength = null;
		}

		/// <summary>
		/// Returns the headers associated with this content.
		/// </summary>
		public HttpContentHeaderCollection Headers
		{
			get
			{
				return headers;
			}
		}

		/// <summary>
		/// Compresses the content and stores it in an internal buffer.
		/// </summary>
		/// <returns>The length of the compressed data.</returns>
		public IAsyncOperationWithProgress<ulong, ulong> BufferAllAsync()
		{
			return AsyncInfo.Run<ulong, ulong>(async (cancellationToken, progress) =>
			{
				if (bufferedData != null) return bufferedData.Length;

				using (var outputStream = new System.IO.MemoryStream())
				{
					await WriteCompressedContentToStream(outputStream);
					bufferedData = outputStream.ToArray().AsBuffer();
				}
					
				// Report progress.
				progress.Report(bufferedData.Length);
				headers.ContentLength = bufferedData.Length;

				// Just return the size in bytes.
				return bufferedData.Length;
			});
		}

		/// <summary>
		/// Compresses the data and returns it as an <see cref="IBuffer"/>.
		/// </summary>
		/// <returns>An <see cref="IBuffer"/> implementation.</returns>
		public IAsyncOperationWithProgress<IBuffer, ulong> ReadAsBufferAsync()
		{
			return AsyncInfo.Run<IBuffer, ulong>((cancellationToken, progress) =>
			{
				return Task<IBuffer>.Run(async () =>
				{
					await BufferAllAsync().AsTask().ConfigureAwait(false);

					using (DataWriter writer = new DataWriter())
					{
						writer.WriteBuffer(bufferedData);

						// Make sure that the DataWriter destructor does not free the buffer.
						IBuffer buffer = writer.DetachBuffer();

						// Report progress.
						progress.Report(buffer.Length);

						return buffer;
					}
				});
			});
		}

		/// <summary>
		/// Compresses the content and returns it as an input stream.
		/// </summary>
		/// <returns>A <see cref="IInputStream"/> implementation containing the compresssed data.</returns>
		public IAsyncOperationWithProgress<IInputStream, ulong> ReadAsInputStreamAsync()
		{
			return AsyncInfo.Run<IInputStream, ulong>(async (cancellationToken, progress) =>
			{
				await BufferAllAsync().AsTask().ConfigureAwait(false);

				var randomAccessStream = new InMemoryRandomAccessStream();
				try
				{
					using (var writer = new DataWriter(randomAccessStream))
					{
						writer.WriteBuffer(bufferedData);

						uint bytesStored = await writer.StoreAsync().AsTask(cancellationToken);

						// Make sure that the DataWriter destructor does not close the stream.
						writer.DetachStream();

						// Report progress.
						progress.Report(randomAccessStream.Size);

						return randomAccessStream.GetInputStreamAt(0);
					}
				}
				catch
				{
					randomAccessStream?.Dispose();
					throw;
				}
			});
		}

		/// <summary>
		/// Compresses the content and returns it as a string.
		/// </summary>
		/// <returns>A string containing the compressed content.</returns>
		public IAsyncOperationWithProgress<string, ulong> ReadAsStringAsync()
		{
			return AsyncInfo.Run<string, ulong>((cancellationToken, progress) =>
			{
				return Task<string>.Run(async () =>
				{
					await BufferAllAsync().AsTask().ConfigureAwait(false);
					var text = System.Text.UTF8Encoding.UTF8.GetString(bufferedData.ToArray());

					// Report progress (length of string).
					progress.Report((ulong)text.Length);

					return text;
				});
			});
		}

		/// <summary>
		/// Returns the length of the compressed content.
		/// </summary>
		/// <param name="length">An integer to provde the length in.</param>
		/// <returns>The length of the compressed content.</returns>
		public bool TryComputeLength(out ulong length)
		{
			BufferAllAsync().AsTask().Wait();
			if (bufferedData != null)
			{
				length = bufferedData.Length;
				return true;
			}

			length = 0;
			return false;
		}

		/// <summary>
		/// Writes the compressed content to the output stream.
		/// </summary>
		/// <param name="outputStream">The stream to write to.</param>
		/// <returns>The number of bytes written.</returns>
		public IAsyncOperationWithProgress<ulong, ulong> WriteToStreamAsync(IOutputStream outputStream)
		{
			return AsyncInfo.Run<ulong, ulong>(async (cancellationToken, progress) =>
			{
				using (DataWriter writer = new DataWriter(outputStream))
				{
					if (bufferedData == null)
						await BufferAllAsync().AsTask().ConfigureAwait(false);

					headers.ContentLength = bufferedData.Length;

					writer.WriteBuffer(bufferedData);
					await writer.StoreAsync().AsTask().ConfigureAwait(false);
					// Make sure that DataWriter destructor does not close the stream.
					writer.DetachStream();

					// Report progress.
					progress.Report(bufferedData.Length);

					return bufferedData.Length;
				}
			});
		}

		/// <summary>
		/// Disposes this content and all internal resources.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Dispose(true);
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		/// <param name="isDisposing">True if this method is being called explicitly from user code, otherwise false if it being called from the finalizer by the GC.</param>
		protected virtual void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				if (originalContent != null)
				{
					originalContent?.Dispose();
					originalContent = null;

					bufferedData = null;
				}
			}
		}

		private async Task WriteCompressedContentToStream(Stream outputStream)
		{
			Stream compressedStream = null;

			if (String.Compare(encodingType, "gzip", true) == 0)
				compressedStream = new GZipStream(outputStream, CompressionMode.Compress, leaveOpen: true);
			else if (String.Compare(encodingType, "deflate", true) == 0)
				compressedStream = new DeflateStream(outputStream, CompressionMode.Compress, leaveOpen: true);

			using (var stream = await originalContent.ReadAsInputStreamAsync().AsTask().ConfigureAwait(false))
			{
				using (var sourceStream = stream.AsStreamForRead())
				{
					try
					{
						sourceStream.CopyTo(compressedStream);
					}
					finally
					{
						compressedStream?.Dispose();
					}
				}
			}
		}

	}
}