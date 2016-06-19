using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.ClientPipeline.Portable.Tests
{
	public static class TestSecrets
	{
		public const string OAuth2AccessTokenUrl = "http://postie-loyalty-stg-web.elasticbeanstalk.com/api/v1/oauth/access-token";
		public const string OAuth2AuthorisationUrl = "http://postie-loyalty-stg-web.elasticbeanstalk.com/api/v1/oauth/authorize";
		public const string OAuth2RedirectUrl = "http://postie-loyalty-stg-web.elasticbeanstalk.com/reflect";

		public const string OAuth2TestUrl1 = "http://postie-loyalty-stg-web.elasticbeanstalk.com/api/v1/rewarddefinitions";
		public const string OAuth2TestUrl2 = "http://postie-loyalty-stg-web.elasticbeanstalk.com/api/v1/members";

		public const string OAuth2CredentialId = "1478c028a16709cb32d8b1a69ccca032ca1d9ef5";
		public const string OAuth2CredentialSecret = "7b42b156f9092f8ba8a247c6d45d775ad2dc23c7";
	
	}
}