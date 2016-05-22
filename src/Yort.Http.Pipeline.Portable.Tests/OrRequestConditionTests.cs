using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Portable.Tests
{
	[TestClass]
	public class OrRequestConditionTests
	{

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void OrRequestCondition_Constructor_ThrowsWhenChildConditionsNull()
		{
			var orCondition = new OrRequestCondition(null);
		}

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		public void OrRequestCondition_Constructor_ConstructsOkWithEmptyChildConditions()
		{
			var orCondition = new OrRequestCondition(new IRequestCondition[] { });
		}

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		public void OrRequestCondition_Constructor_ReturnsTrueIfAnyChildConditionsPass()
		{
			var condition1 = new AuthorityHttpRequestCondition();
			condition1.AddAuthority("sometestsite");

			var condition2 = new RequestContentTypeCondition();
			condition2.AddContentMediaType(MediaTypes.TextPlain);

			var andCondition = new OrRequestCondition(new IRequestCondition[] { condition1, condition2 });

			var testRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://sometestsite/testendpoint");
			testRequest.Content = new System.Net.Http.StringContent("AAAABBBBCCCCDDD", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson);

			Assert.IsTrue(andCondition.ShouldProcess(testRequest));
		}

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		public void OrRequestCondition_Constructor_ReturnsFalseIfNoChildConditionDoesNotPass()
		{
			var condition1 = new AuthorityHttpRequestCondition();
			condition1.AddAuthority("sometestsite");

			var condition2 = new RequestContentTypeCondition();
			condition2.AddContentMediaType(MediaTypes.TextPlain);

			var andCondition = new OrRequestCondition(new IRequestCondition[] { condition1, condition2 });

			var testRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://someothertestsite/testendpoint");
			testRequest.Content = new System.Net.Http.StringContent("AAAABBBBCCCCDDD", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson);

			Assert.IsFalse(andCondition.ShouldProcess(testRequest));
		}

	}
}