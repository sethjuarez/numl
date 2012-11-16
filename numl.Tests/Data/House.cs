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
 
 Data gathered from 
 http://www.decisiontrees.net/?q=node/21
 
*/

using System;
using numl.Model;

namespace numl.Data
{
	public enum District
	{
		Suburban,
		Rural,
		Urban
	}

	public enum HouseType
	{
		Detached,
		SemiDetached,
		Terrace
	}

	public enum Income
	{
		High,
		Low
	}

	public class House
	{
		[Feature]
		public District District { get; set; }
		[Feature]
		public HouseType HouseType { get; set; }
		[Feature]
		public Income Income { get; set; }
		[Feature]
		public bool PreviousCustomer { get; set; }
		[Label]
		public bool Response { get; set; }

		public static House[] GetData()
		{
			return new House[]
			{
				new House { District = District.Suburban, HouseType= HouseType.Detached, Income = Income.High, PreviousCustomer = false, Response = false },
				new House { District = District.Suburban, HouseType= HouseType.Detached, Income = Income.High, PreviousCustomer = true, Response = false },
				new House { District = District.Rural, HouseType= HouseType.Detached, Income = Income.High, PreviousCustomer = false, Response = true },
				new House { District = District.Urban, HouseType= HouseType.SemiDetached, Income = Income.High, PreviousCustomer = false, Response = true },
				new House { District = District.Urban, HouseType= HouseType.SemiDetached, Income = Income.Low, PreviousCustomer = false, Response = true },
				new House { District = District.Urban, HouseType= HouseType.SemiDetached, Income = Income.Low, PreviousCustomer = true , Response = false },
				new House { District = District.Rural, HouseType= HouseType.SemiDetached, Income = Income.Low, PreviousCustomer = true, Response = true },
				new House { District = District.Suburban, HouseType= HouseType.Terrace, Income = Income.High, PreviousCustomer = false, Response = false },
				new House { District = District.Suburban, HouseType= HouseType.SemiDetached, Income = Income.Low, PreviousCustomer = false, Response = true },
				new House { District = District.Urban, HouseType= HouseType.Terrace, Income = Income.Low, PreviousCustomer = false, Response = true },
				new House { District = District.Suburban, HouseType= HouseType.Terrace, Income = Income.Low, PreviousCustomer = true, Response = true },
				new House { District = District.Rural, HouseType= HouseType.Terrace, Income = Income.High, PreviousCustomer = true, Response = true },
				new House { District = District.Rural, HouseType= HouseType.Detached, Income = Income.Low, PreviousCustomer = false, Response = true },
				new House { District = District.Urban, HouseType= HouseType.Terrace, Income = Income.High, PreviousCustomer = true, Response = false }
			};
		}
	}
}
