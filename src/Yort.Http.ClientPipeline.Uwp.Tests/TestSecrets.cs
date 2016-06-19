using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Uwp.Tests
{
	public static class TestSecrets
	{
		public const string OAuth2AccessTokenUrl = "http://postie-loyalty-stg-web.elasticbeanstalk.com/api/v1/oauth/access-token";
		public const string OAuth2AuthorisationUrl = "http://postie-loyalty-stg-web.elasticbeanstalk.com/api/v1/oauth/authorize";
		public const string OAuth2RedirectUrl = "http://postie-loyalty-stg-web.elasticbeanstalk.com/reflect";

	}
}