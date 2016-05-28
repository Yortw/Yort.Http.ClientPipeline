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
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MockMessageHandler()
		{
			Helper.Throw();
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
		/// Returns a read only collection of requests processed by this handlers <see cref="SendAsync(System.Net.Http.HttpRequestMessage, CancellationToken)"/> method.
		/// </summary>
		public ReadOnlyCollection<HttpRequestMessage> Requests
		{
			get
			{
				Helper.Throw();
				return null;
			}
		}

		/// <summary>
		/// Checks for configured responses and returns one, or if none is found returns a generic 404 response.
		/// </summary>
		/// <param name="request">A <see cref="HttpResponseMessage"/> to process.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/>. Required by the interface but not actually used by this instance.</param>
		/// <returns></returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Helper.Throw();
			return null;
		}

		/// <summary>
		/// Removes all requests from the <see cref="Requests"/>, resetting the collection to an empty state.
		/// </summary>
		public void ClearRequests()
		{
			Helper.Throw();
		}

		/// <summary>
		/// Adds a static response for a GET to the specified request uri.
		/// </summary>
		/// <param name="requestUri">The uri of the requests that will receive this response.</param>
		/// <param name="responseMessage">An <see cref="HttpResponseMessage"/> instance that will be returned to clients who make requests agaisnt the <paramref name="requestUri"/>.</param>
		public void AddFixedResponse(Uri requestUri, HttpResponseMessage responseMessage)
		{
			Helper.Throw();
		}

		/// <summary>
		/// Adds a static response for a specific request uri.
		/// </summary>
		/// <param name="httpMethod">The HTTP Method (GET, POST, PUT, DELETE, HEAD, OPTIONS etc) to associated with the <paramref name="responseMessage"/>. This argument is not case-sensitive.</param>
		/// <param name="requestUri">The uri of the requests that will receive this response. This argument is case-sensitve, only requests matching the url precisley will have the correct response returned.</param>
		/// <param name="responseMessage">An <see cref="HttpResponseMessage"/> instance that will be returned to clients who make requests agaisnt the <paramref name="requestUri"/>.</param>
		public void AddFixedResponse(string httpMethod, Uri requestUri, HttpResponseMessage responseMessage)
		{
			Helper.Throw();
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