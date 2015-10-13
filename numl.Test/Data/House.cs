/*
 Data gathered from 
 http://www.decisiontrees.net/?q=node/21
*/

using System;
using numl.Model;

namespace numl.Tests.Data
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
