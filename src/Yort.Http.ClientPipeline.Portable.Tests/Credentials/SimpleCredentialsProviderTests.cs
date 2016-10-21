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
	public class SimpleCredentialsProviderTests
	{

		#region Constructor Tests

		[TestMethod]
		[TestCategory(nameof(SimpleCredentialProvider))]
		[TestCategory("Credentials")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SimpleCredentialProvider_Constructor_ThrowsOnNullCredentials()
		{
			var credProvider = new SimpleCredentialProvider(null);
		}

		[TestMethod]
		[TestCategory(nameof(SimpleCredentialProvider))]
		[TestCategory("Credentials")]
		public void SimpleCredentialProvider_Constructor_ConstructsWithNonNullCreds()
		{
			var creds = new SimpleCredentials()
			{
				Identifier = "yort@aserver.com",
				Secret = "P@ssword123"
			};

			var credProvider = new SimpleCredentialProvider(creds);
		}

		#endregion

		[TestMethod]
		[TestCategory(nameof(SimpleCredentialProvider))]
		[TestCategory("Credentials")]
		public async Task SimpleCredentialProvider_Constructor_GetCredentials()
		{
			var creds = new SimpleCredentials()
			{
				Identifier = "yort@aserver.com",
				Secret = "P@ssword123"
			};

			var credProvider = new SimpleCredentialProvider(creds);
			var results = await credProvider.GetCredentials().ConfigureAwait(false);
			Assert.IsNotNull(results);
			Assert.AreEqual("yort@aserver.com", results.Identifier);
			Assert.AreEqual("P@ssword123", results.Secret);
		}

	}
}