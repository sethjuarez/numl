/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Model
{
	public class DateProperty : Property
	{
		public override int Length
		{
			get
			{
				return 8;
			}
		}

		public override double[] ToArray(object o)
		{
			if (o is DateTime)
			{
				DateTime d = (DateTime)o;
				return new double[] { d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, (int)d.DayOfWeek, d.DayOfYear };
			}
			else
				throw new InvalidOperationException("Invalid Date!");
		}

		public override double Convert(object o)
		{
			if (o is DateTime)
			{
				DateTime d = (DateTime)o;
				return d.ToOADate();
			}
			else
				throw new InvalidOperationException("Invalid Date!");
		}
	}
}
