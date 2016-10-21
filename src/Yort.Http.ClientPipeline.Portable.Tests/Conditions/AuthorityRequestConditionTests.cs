using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	[TestClass]
	public class AuthorityRequestConditionTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(AuthorityRequestCondition))]
		[TestCategory("Conditions")]
		public void AuthorityRequestCondition_Constructor_AllowsNullAuthorityList()
		{
			var authorityCondition = new AuthorityRequestCondition(null);
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(AuthorityRequestCondition))]
		public void AuthorityRequestCondition_Constructor_ConstructsWithDefaultConstuctor()
		{
			var authorityCondition = new AuthorityRequestCondition();
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(AuthorityRequestCondition))]
		public void AuthorityRequestCondition_Constructor_ConstructsWithEmptyList()
		{
			var authorityCondition = new AuthorityRequestCondition(new string[] { });
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(AuthorityRequestCondition))]
		public void AuthorityRequestCondition_Constructor_ConstructsWithSingleAuthority()
		{
			var authorityCondition = new AuthorityRequestCondition(new string[] { "www.microsoft.com" });
		}

		[TestMethod]
		[TestCategory("Conditions")]
		[TestCategory(nameof(AuthorityRequestCondition))]
		public void AuthorityRequestCondition_Constructor_ConstructsWithMultipleAuthorities()
		{
			var authorityCondition = new AuthorityRequestCondition(new string[] { "www.microsoft.com", "www.google.com" });
		}

		#endregion

		#region ShouldProcess Tests

		[TestMethod]
		[TestCategory(nameof(AuthorityRequestCondition))]
		[TestCategory("Conditions")]
		public void AuthorityRequestCondition_ShouldProcess_ReturnsTrueForAnyAuthority()
		{
			var authorityCondition = new AuthorityRequestCondition(new string[] { "www.microsoft.com", "www.google.com" });
			Assert.IsTrue(authorityCondition.ShouldProcess(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.microsoft.com/")));
			Assert.IsTrue(authorityCondition.ShouldProcess(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.google.com/")));
		}

		[TestMethod]
		[TestCategory(nameof(AuthorityRequestCondition))]
		[TestCategory("Conditions")]
		public void AuthorityRequestCondition_ShouldProcess_ReturnsFalseForUnsupportedAuthority()
		{
			var authorityCondition = new AuthorityRequestCondition(new string[] { "www.microsoft.com", "www.google.com" });
			Assert.IsFalse(authorityCondition.ShouldProcess(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.yahoo.com/")));
		}

		[TestMethod]
		[TestCategory(nameof(AuthorityRequestCondition))]
		[TestCategory("Conditions")]
		public void AuthorityRequestCondition_ShouldProcess_AddAuthorityWorks()
		{
			var authorityCondition = new AuthorityRequestCondition(new string[] { "www.microsoft.com", "www.google.com" });
			Assert.IsFalse(authorityCondition.ShouldProcess(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.yahoo.com/")));
			authorityCondition.AddAuthority("www.yahoo.com");
			Assert.IsTrue(authorityCondition.ShouldProcess(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.yahoo.com/")));
		}

		[TestMethod]
		[TestCategory(nameof(AuthorityRequestCondition))]
		[TestCategory("Conditions")]
		public void AuthorityRequestCondition_ShouldProcess_RemoveAuthorityWorks()
		{
			var authorityCondition = new AuthorityRequestCondition(new string[] { "www.microsoft.com", "www.google.com" });
			Assert.IsTrue(authorityCondition.ShouldProcess(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.google.com/")));
			authorityCondition.RemoveAuthority("www.google.com");
			Assert.IsFalse(authorityCondition.ShouldProcess(new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://www.google.com/")));
		}

		#endregion

	}
}