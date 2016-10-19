using numl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace numl.Model
{
	/// <summary>Represents a guid property.</summary>
	public class GuidProperty : Property
	{
		/// <summary>Default constructor.</summary>
		public GuidProperty()
			: base()
		{
			// set to default conventions
			Categories = new Guid[] { };
			Type = typeof(Guid);
			Discrete = false;
		}
		
		/// <value>The categories.</value>
		public Guid[] Categories { get; set; }
		
		/// <summary>Preprocess data set to create category list.</summary>
		/// <param name="examples">.</param>
		public override void PreProcess(IEnumerable<object> examples)
		{
			var q = examples.Select(s => (Guid)Ject.Get(s, Name)).Distinct();
			Categories = q.ToArray();
		}
		/// <summary>Convert from number to guid.</summary>
		/// <param name="val">Number.</param>
		/// <returns>Guid.</returns>
		public override object Convert(double val)
		{
			return Categories[(int)val];
		}
		/// <summary>Convert guid to list of numbers.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="o">in guid.</param>
		/// <returns>lazy list of numbers.</returns>
		public override IEnumerable<double> Convert(object o)
		{
			// check for valid cateogry list
			if (Categories == null || Categories.Length == 0)
				throw new InvalidOperationException(string.Format("{0} categories do not exist.", Name));
			
			for (int i = 0; i < Categories.Length; i++)
			{
				if (Categories[i] == (o as Guid?))
				{
					yield return i;
				}
			}
		}
		/// <summary>
		/// Equality test
		/// </summary>
		/// <param name="obj">object to compare</param>
		/// <returns>equality</returns>
		public override bool Equals(object obj)
		{
			if (base.Equals(obj) && obj is GuidProperty)
			{
				var p = obj as GuidProperty;
				if (Categories.Length == p.Categories.Length)
				{
					for (int i = 0; i < Categories.Length; i++)
						if (Categories[i] != p.Categories[i])
							return false;
					
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Return hash
		/// </summary>
		/// <returns>hash</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
