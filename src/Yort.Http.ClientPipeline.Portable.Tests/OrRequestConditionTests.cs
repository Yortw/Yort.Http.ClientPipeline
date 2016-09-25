using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class OrRequestConditionTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		[TestCategory("Conditions")]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void OrRequestCondition_Constructor_ThrowsWhenChildConditionsNull()
		{
			var orCondition = new OrRequestCondition(null);
		}

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		[TestCategory("Conditions")]
		public void OrRequestCondition_Constructor_ConstructsOkWithEmptyChildConditions()
		{
			var orCondition = new OrRequestCondition(new IRequestCondition[] { });
		}

		#endregion

		#region ShouldProcess Tests

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		[TestCategory("Conditions")]
		public void OrRequestCondition_ShouldProcess_ReturnsTrueIfAnyChildConditionsPass()
		{
			var condition1 = new AuthorityRequestCondition();
			condition1.AddAuthority("sometestsite");

			var condition2 = new RequestContentMediaTypeCondition();
			condition2.AddContentMediaType(MediaTypes.TextPlain);

			var orCondition = new OrRequestCondition(new IRequestCondition[] { condition1, condition2 });

			var testRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://sometestsite/testendpoint");
			testRequest.Content = new System.Net.Http.StringContent("AAAABBBBCCCCDDD", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson);

			Assert.IsTrue(orCondition.ShouldProcess(testRequest));
		}

		[TestMethod]
		[TestCategory(nameof(OrRequestCondition))]
		[TestCategory("Conditions")]
		public void OrRequestCondition_ShouldProcess_ReturnsFalseIfNoChildConditionDoesNotPass()
		{
			var condition1 = new AuthorityRequestCondition();
			condition1.AddAuthority("sometestsite");

			var condition2 = new RequestContentMediaTypeCondition();
			condition2.AddContentMediaType(MediaTypes.TextPlain);

			var orCondition = new OrRequestCondition(new IRequestCondition[] { condition1, condition2 });

			var testRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://someothertestsite/testendpoint");
			testRequest.Content = new System.Net.Http.StringContent("AAAABBBBCCCCDDD", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson);

			Assert.IsFalse(orCondition.ShouldProcess(testRequest));
		}

		#endregion

	}
}