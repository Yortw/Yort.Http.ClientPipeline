using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Http.ClientPipeline
{
	internal static class Helper
	{
		internal static void Throw()
		{
			throw new InvalidOperationException("Incorrect reference, this assembly should never be loaded at runtime. Please reference the platform specific (Yort.Http.ClientPipeline) assembly for your application. The portable assembly should only be referenced from other PCLs.");
		}
	}
}