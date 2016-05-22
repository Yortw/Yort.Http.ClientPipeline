using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline
{
	/// <summary>
	/// Extensions for <see cref="System.Net.Http.HttpClient"/>.
	/// </summary>
	public static class HttpClientExtensions
	{

		#region PatchAsync Overloads

		/// <summary>
		/// Send a PATCH request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">A string containing the uri to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
		{
			return await PatchAsync(client, new Uri(requestUri), content, System.Threading.CancellationToken.None).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a PATCH request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="System.Net.Http.HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="System.Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="System.Net.Http.HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
		{
			return await PatchAsync(client, requestUri, content, System.Threading.CancellationToken.None).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a PATCH request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			return await PatchAsync(client, new Uri(requestUri), content, cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a PATCH request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="System.Net.Http.HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="System.Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="System.Net.Http.HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			var method = new HttpMethod("PATCH");

			var request = new HttpRequestMessage(method, requestUri)
			{
				Content = content
			};

			return await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}

		#endregion

		#region HeadAsync Overloads

		/// <summary>
		/// Send a Head request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">A string containing the uri to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public async static Task<HttpResponseMessage> HeadAsync(this HttpClient client, string requestUri, HttpContent content)
		{
			return await HeadAsync(client, new Uri(requestUri), content, System.Threading.CancellationToken.None).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a Head request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="System.Net.Http.HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="System.Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="System.Net.Http.HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		public async static Task<HttpResponseMessage> HeadAsync(this HttpClient client, Uri requestUri, HttpContent content)
		{
			return await HeadAsync(client, requestUri, content, System.Threading.CancellationToken.None).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a Head request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public async static Task<HttpResponseMessage> HeadAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			return await HeadAsync(client, new Uri(requestUri), content, cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a Head request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="System.Net.Http.HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="System.Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="System.Net.Http.HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		public async static Task<HttpResponseMessage> HeadAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			var method = new HttpMethod("HEAD");

			var request = new HttpRequestMessage(method, requestUri)
			{
				Content = content
			};

			return await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}

		#endregion

		#region OptionsAsync Overloads

		/// <summary>
		/// Send a Options request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">A string containing the uri to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public async static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, string requestUri, HttpContent content)
		{
			return await OptionsAsync(client, new Uri(requestUri), content, System.Threading.CancellationToken.None).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a Options request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="System.Net.Http.HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="System.Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="System.Net.Http.HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		public async static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, Uri requestUri, HttpContent content)
		{
			return await OptionsAsync(client, requestUri, content, System.Threading.CancellationToken.None).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a Options request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public async static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			return await OptionsAsync(client, new Uri(requestUri), content, cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Send a Options request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="System.Net.Http.HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="System.Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="System.Net.Http.HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="System.Net.Http.HttpResponseMessage"/> from the server.</returns>
		public async static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			var method = new HttpMethod("OPTIONS");

			var request = new HttpRequestMessage(method, requestUri)
			{
				Content = content
			};

			return await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}

		#endregion

	}
}