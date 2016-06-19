using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a PATCH request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a PATCH request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a PATCH request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
		public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public static Task<HttpResponseMessage> HeadAsync(this HttpClient client, string requestUri, HttpContent content)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a Head request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		public static Task<HttpResponseMessage> HeadAsync(this HttpClient client, Uri requestUri, HttpContent content)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a Head request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public static Task<HttpResponseMessage> HeadAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a Head request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
		public static Task<HttpResponseMessage> HeadAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, string requestUri, HttpContent content)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a Options request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		public static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, Uri requestUri, HttpContent content)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a Options request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Send a Options request to the specified Uri as an asynchronous operation.
		/// </summary>
		/// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
		/// <param name="requestUri">The <see cref="Uri"/> to send the request to.</param>
		/// <param name="content">The <see cref="HttpContent"/> request content to send to the server.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
		/// <returns>A <see cref="System.Threading.Tasks.Task{T}"/> whose result is the <see cref="HttpResponseMessage"/> from the server.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "requestUri")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "content")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "client")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
		public static Task<HttpResponseMessage> OptionsAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
		}

		#endregion

	}
}