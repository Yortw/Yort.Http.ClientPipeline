using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline
{
	//TODO: Allow timeouts to specific urls? Use cancellation token to throw?

	/// <summary>
	/// Provides configured responses to requests, used for testing purposes to avoid network/API dependencies during testing.
	/// </summary>
	/// <remarks>
	/// <para>This class is not thread-safe. You should not add responses or response handlers while calls are in progress. Ideally, setup all responses first then perform tests.</para>
	/// </remarks>
	/// <see cref="MockResponseHandler"/>
	public class MockMessageHandler : System.Net.Http.DelegatingHandler
	{

		private IDictionary<string, HttpResponseMessage> _FixedResponses;
		private IList<MockResponseHandler> _DynamicResponses;

		private readonly IList<HttpRequestMessage> _Requests;
		private readonly ReadOnlyCollection<HttpRequestMessage> _ReadOnlyRequests;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public MockMessageHandler()
		{
			_FixedResponses = new Dictionary<string, HttpResponseMessage>();
			_DynamicResponses = new List<MockResponseHandler>();
			_Requests = new List<HttpRequestMessage>();
			_ReadOnlyRequests = new ReadOnlyCollection<HttpRequestMessage>(_Requests);
		}

		/// <summary>
		/// Sets or returns the default <see cref="HttpResponseMessage"/> to return when no response is found.
		/// </summary>
		/// <remarks>
		/// <para>If this property is null, the system will generate a generic 404 Not Found response. This property only needs to be set if you require a non-standard response.</para>
		/// </remarks>
		public HttpResponseMessage DefaultResponse
		{
			get;
			set;
		}

		/// <summary>
		/// Returns a read only collection of requests processed by this handlers <see cref="SendAsync(HttpRequestMessage, CancellationToken)"/> method.
		/// </summary>
		public ReadOnlyCollection<HttpRequestMessage> Requests
		{
			get
			{
				return _ReadOnlyRequests;
			}
		}

		/// <summary>
		/// Removes all requests from the <see cref="Requests"/>, resetting the collection to an empty state.
		/// </summary>
		public void ClearRequests()
		{
			_Requests.Clear();
		}

		/// <summary>
		/// Adds a static response for a GET to the specified request uri.
		/// </summary>
		/// <param name="requestUri">The uri of the requests that will receive this response.</param>
		/// <param name="responseMessage">An <see cref="HttpResponseMessage"/> instance that will be returned to clients who make requests agaisnt the <paramref name="requestUri"/>.</param>
		public void AddFixedResponse(Uri requestUri, HttpResponseMessage responseMessage)
		{
			AddFixedResponse(null, requestUri, responseMessage);
		}

		/// <summary>
		/// Adds a static response for a specific request uri.
		/// </summary>
		/// <param name="httpMethod">The HTTP Method (GET, POST, PUT, DELETE, HEAD, OPTIONS etc) to associated with the <paramref name="responseMessage"/>. This argument is not case-sensitive.</param>
		/// <param name="requestUri">The uri of the requests that will receive this response. This argument is case-sensitve, only requests matching the url precisley will have the correct response returned.</param>
		/// <param name="responseMessage">An <see cref="HttpResponseMessage"/> instance that will be returned to clients who make requests agaisnt the <paramref name="requestUri"/>.</param>
		public void AddFixedResponse(string httpMethod, Uri requestUri, HttpResponseMessage responseMessage)
		{
			if (String.IsNullOrWhiteSpace(httpMethod)) httpMethod = "GET";

			_FixedResponses[GetRequestKey(httpMethod, requestUri)] = responseMessage;
		}

		/// <summary>
		/// Checks for configured responses and returns one, or if none is found returns a generic 404 response.
		/// </summary>
		/// <param name="request">A <see cref="HttpResponseMessage"/> to process.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/>. Required by the interface but not actually used by this instance.</param>
		/// <returns></returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var clonedRequest = await CloneRequest(request).ConfigureAwait(false);
			_Requests.Add(clonedRequest);
			
			//If there's a simple fixed response, return it.
			var key = GetRequestKey(request.Method.Method, request.RequestUri);
			if (_FixedResponses.ContainsKey(key)) return await CloneResponse(_FixedResponses[key], clonedRequest);

			cancellationToken.ThrowIfCancellationRequested();

			//If there are dynamic handlers that can process this request, return the response from the first match.
			foreach (var handler in _DynamicResponses)
			{
				if (handler.CanHandleRequest == null || handler.CanHandleRequest(request))
					return handler.HandleRequest(request);

				cancellationToken.ThrowIfCancellationRequested();
			}

			//We have no configured response, so return the default or a 404.
			var NotFoundResponse = await CloneResponse(this.DefaultResponse, request) ?? new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
			{
				RequestMessage = request
			};
			return NotFoundResponse;
		}

		private static string GetRequestKey(string httpMethod, Uri requestUri)
		{
			return httpMethod.ToUpperInvariant() + ":" + requestUri.ToString();
		}

		private static async Task<HttpResponseMessage> CloneResponse(HttpResponseMessage response, HttpRequestMessage request)
		{
			if (response == null) return null;

			var retVal = new HttpResponseMessage(response.StatusCode)
			{
				ReasonPhrase = response.ReasonPhrase,
				Version = response.Version,
				RequestMessage = request
			};
			
			CopyHeaders(response.Headers, retVal.Headers);
			await CopyContent(response, retVal).ConfigureAwait(false);

			return retVal;
		}

		private static async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
		{
			var retVal = new HttpRequestMessage(request.Method, request.RequestUri);
			foreach (var header in request.Headers)
			{
				retVal.Headers.Add(header.Key, header.Value);
			}

			if (request.Content != null)
			{
				var content = new ByteArrayContent(await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false));
				foreach (var header in request.Content.Headers)
				{
					content.Headers.Add(header.Key, header.Value);
				}
				retVal.Content = content;
			}

			return retVal;
		}

		private static void CopyHeaders(HttpHeaders source, HttpHeaders destination)
		{
			foreach (var header in source)
			{
				destination.TryAddWithoutValidation(header.Key, header.Value);
			}
		}
		
		private static async Task CopyContent(HttpResponseMessage source, HttpResponseMessage destination)
		{
			if (source.Content == null)
			{
				destination.Content = null;
				return;
			}

			//If the content is a stream, it may only be readable once, so convert it to a byte
			//array content to allow for multiple requests.
			var streamContent = source.Content as System.Net.Http.StreamContent;
			if (streamContent != null)
			{
				var newContentData = await streamContent.ReadAsByteArrayAsync().ConfigureAwait(false);
				source.Content = new System.Net.Http.ByteArrayContent(newContentData);
			}
			
			var copiedContents = await source.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			destination.Content = new ByteArrayContent(copiedContents);
			CopyContentHeaders(source.Content.Headers, destination.Content.Headers);
		}

		private static void CopyContentHeaders(HttpContentHeaders source, HttpContentHeaders destination)
		{
			foreach (var header in source)
			{
				destination.TryAddWithoutValidation(header.Key, header.Value);
			}
		}
	}

	/// <summary>
	/// A dynamic mock handler for web requests. Allows local code to generate responses to requests without accessing the network.
	/// </summary>
	/// <remarks>
	/// <para>Use an instance of this class with the <see cref="MockMessageHandler"/> when you need to generate part or all of the response to a request rather than provding a static response.</para>
	/// <para>If multiple handlers can process a request, only the first one is used.</para>
	/// </remarks>
	/// <seealso cref="MockResponseHandler"/>
	public class MockResponseHandler
	{
		/// <summary>
		/// A function that accepts an <see cref="HttpRequestMessage"/> and returns a boolean indicating if this instance can process that request and return a valid response.
		/// </summary>
		/// <remarks>
		/// <para>If this property is null, the system will act as if the function had returned true, so this handler will process ALL requests.</para>
		/// </remarks>
		public Func<HttpRequestMessage, bool> CanHandleRequest { get; set; }
		/// <summary>
		/// A function that accepts an <see cref="HttpRequestMessage"/> and returns an appropriate <see cref="HttpResponseMessage"/>. 
		/// </summary>
		public Func<HttpRequestMessage, HttpResponseMessage> HandleRequest { get; set; }
	}
}