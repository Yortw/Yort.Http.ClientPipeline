using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yort.Http.ClientPipeline;

namespace Yort.HttpClient.Pipeline.Portable.Tests.Credentials
{
	[TestClass]
	public class SimpleCredentialsTests
	{
		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(SimpleCredentials))]
		[TestCategory("Credentials")]
		public void SimpleCredentials_Constructor_ConstructsOk()
		{
			var credentials = new SimpleCredentials();
		}

		#endregion

		#region Property Tests

		[TestMethod]
		[TestCategory(nameof(SimpleCredentials))]
		[TestCategory("Credentials")]
		public void SimpleCredentials_Identifier_GetsAndSets()
		{
			var credentials = new SimpleCredentials();
			credentials.Identifier = "yort@aserver.com";
			Assert.AreEqual("yort@aserver.com", credentials.Identifier);
		}

		[TestMethod]
		[TestCategory(nameof(SimpleCredentials))]
		[TestCategory("Credentials")]
		public void SimpleCredentials_Secret_GetsAndSets()
		{
			var credentials = new SimpleCredentials();
			credentials.Secret = "P@ssword123"; // Never use a password this dumb.
			Assert.AreEqual("P@ssword123", credentials.Secret);
		}

		#endregion

		#region Dispose Tests

		[TestMethod]
		[TestCategory(nameof(SimpleCredentials))]
		[TestCategory("Credentials")]
		public void SimpleCredentials_Dispose_ClearsCredentials()
		{
			var credentials = new SimpleCredentials();
			credentials.Identifier = "yort@aserver.com";
			credentials.Secret = "P@ssword123";
			credentials.Dispose();
			Assert.IsNull(credentials.Identifier);
			Assert.IsNull(credentials.Secret);
		}

		#endregion

	}
}