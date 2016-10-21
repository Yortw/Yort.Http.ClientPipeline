using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class RequestContentMediaTypeConditionTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(RequestContentMediaTypeCondition))]
		[TestCategory("Conditions")]
		public void RequestContentMediaTypeCondition_Constructor_AllowsNullMediaList()
		{
			var condition = new RequestContentMediaTypeCondition(null);
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(RequestContentMediaTypeCondition))]
		public void RequestContentMediaTypeCondition_Constructor_ConstructsOkWithEmptyChildConditions()
		{
			var condition = new RequestContentMediaTypeCondition(new string[] { });
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(RequestContentMediaTypeCondition))]
		public void RequestContentMediaTypeCondition_Constructor_ConstructsWithSingleMediaType()
		{
			var condition = new RequestContentMediaTypeCondition(new string[] { MediaTypes.ApplicationJson });
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(RequestContentMediaTypeCondition))]
		public void RequestContentMediaTypeCondition_Constructor_ConstructsWithMultipleMediaTypes()
		{
			var condition = new RequestContentMediaTypeCondition(new string[] { MediaTypes.ApplicationJson, MediaTypes.ApplicationXml });
		}

		#endregion

		#region ShouldProcess Tests

		[TestMethod]
		[TestCategory(nameof(RequestContentMediaTypeCondition))]
		[TestCategory("Conditions")]
		public void RequestContentMediaTypeCondition_ShouldProcess_ReturnsTrueForSupportedMediaType()
		{
			var condition = new RequestContentMediaTypeCondition(new string[] { MediaTypes.ApplicationJson, MediaTypes.ApplicationXml });
			var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://www.someserver.com");
			request.Content = new StringContent("<test />");
			request.Content.Headers.ContentType.MediaType = MediaTypes.ApplicationXml;
			Assert.IsTrue(condition.ShouldProcess(request));
		}

		[TestMethod]
		[TestCategory(nameof(RequestContentMediaTypeCondition))]
		[TestCategory("Conditions")]
		public void RequestContentMediaTypeCondition_ShouldProcess_ReturnsFalseIfAnyChildConditionDoesNotPass()
		{
			var condition = new RequestContentMediaTypeCondition(new string[] { MediaTypes.ApplicationJson, MediaTypes.ApplicationXml });
			var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.someserver.com");
			request.Content = new StringContent("a test string");
			request.Content.Headers.ContentType.MediaType = MediaTypes.TextPlain;
			Assert.IsFalse(condition.ShouldProcess(request));
		}

		#endregion

	}
}