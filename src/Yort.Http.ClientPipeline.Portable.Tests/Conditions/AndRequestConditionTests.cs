using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class AndRequestConditionTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(AndRequestCondition))]
		[TestCategory("Conditions")]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void AndRequestCondition_Constructor_ThrowsWhenChildConditionsNull()
		{
			var andCondition = new AndRequestCondition(null);
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(AndRequestCondition))]
		public void AndRequestCondition_Constructor_ConstructsOkWithEmptyChildConditions()
		{
			var andCondition = new AndRequestCondition(new IRequestCondition[] { });
		}

		#endregion

		#region ShouldProcess Tests

		[TestMethod]
		[TestCategory(nameof(AndRequestCondition))]
		[TestCategory("Conditions")]
		public void AndRequestCondition_ShouldProcess_ReturnsTrueIfAllChildConditionsPass()
		{
			var condition1 = new AuthorityRequestCondition();
			condition1.AddAuthority("sometestsite");

			var condition2 = new RequestContentMediaTypeCondition();
			condition2.AddContentMediaType(MediaTypes.TextPlain);

			var andCondition = new AndRequestCondition(new IRequestCondition[] { condition1, condition2 });

			var testRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://sometestsite/testendpoint");
			testRequest.Content = new System.Net.Http.StringContent("AAAABBBBCCCCDDD", System.Text.UTF8Encoding.UTF8, MediaTypes.TextPlain);

			Assert.IsTrue(andCondition.ShouldProcess(testRequest));
		}

		[TestMethod]
		[TestCategory(nameof(AndRequestCondition))]
		[TestCategory("Conditions")]
		public void AndRequestCondition_ShouldProcess_ReturnsFalseIfAnyChildConditionDoesNotPass()
		{
			var condition1 = new AuthorityRequestCondition();
			condition1.AddAuthority("sometestsite");

			var condition2 = new RequestContentMediaTypeCondition();
			condition2.AddContentMediaType(MediaTypes.TextPlain);

			var andCondition = new AndRequestCondition(new IRequestCondition[] { condition1, condition2 });

			var testRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://sometestsite/testendpoint");
			testRequest.Content = new System.Net.Http.StringContent("AAAABBBBCCCCDDD", System.Text.UTF8Encoding.UTF8, MediaTypes.ApplicationJson);

			Assert.IsFalse(andCondition.ShouldProcess(testRequest));
		}

		#endregion

	}
}