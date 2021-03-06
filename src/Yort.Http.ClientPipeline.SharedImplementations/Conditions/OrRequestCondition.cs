﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Yort.Http.ClientPipeline
{
	/// <summary>
	/// An aggregate <see cref="IRequestCondition"/>, returns true if ANY child conditions are true.
	/// </summary>
	public class OrRequestCondition : IRequestCondition
	{
		private IEnumerable<IRequestCondition> _ChildConditions;

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="childConditions">The enumerable set of child conditions that must all be true for this condition to be true.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="childConditions"/> is null.</exception>
		public OrRequestCondition(IEnumerable<IRequestCondition> childConditions)
		{
			if (childConditions == null) throw new ArgumentNullException(nameof(childConditions));

			_ChildConditions = childConditions;
		}

		/// <summary>
		/// Returns true if all child conditions return true for the specified <paramref name="requestMessage"/>.
		/// </summary>
		/// <param name="requestMessage">The <see cref="HttpRequestMessage"/> to analyse.</param>
		/// <returns>True if all child conditions are true for the specified request.</returns>
		public bool ShouldProcess(HttpRequestMessage requestMessage)
		{
			return _ChildConditions.Any((c) => c.ShouldProcess(requestMessage));
		}
	}
}