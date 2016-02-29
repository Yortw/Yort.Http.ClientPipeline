using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Http.Pipeline.Portable
{
	internal static class TaskEx
	{
		/// <summary>
		/// Returns a completed task with the specified result.
		/// </summary>
		/// <typeparam name="T">The type of the result value.</typeparam>
		/// <param name="result">The value to return as the task result.</param>
		/// <returns>A completed task with the specified result value.</returns>
		public static Task<T> FromResult<T>(T result)
		{
			var tcs = new TaskCompletionSource<T>();
			tcs.SetResult(result);
			return tcs.Task;
		}
	}
}