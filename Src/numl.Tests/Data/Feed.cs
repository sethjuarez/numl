using System;
using numl.Model;

namespace numl.Tests.Data
{
	public class Feed
	{
		[Feature]
		public int Cluster { get; set; }
		[Feature]
		public string Content { get; set; }
		[Feature]
		public double Number { get; set; }
		[Label]
		public bool Label { get; set; }

		public static Feed[] GetData()
		{
			return new Feed[]
			{
				new Feed { Label = true, Cluster = 9, Number = 0, Content="the quick 5 brown fox" },
				new Feed { Label = true, Cluster = 8, Number = 1, Content="eating is for dogs (10)" },
				new Feed { Label = true, Cluster = 7, Number = 2, Content="never eat yellow 52 snow" },
				new Feed { Label = true, Cluster = 6, Number = 3, Content="kids like 3.5 robotic arms" },
				new Feed { Label = true, Cluster = 5, Number = 4, Content="don't kill nice animals" },
				new Feed { Label = false, Cluster = 4, Number = 5, Content="buy things {23.32} with money not fame" },
				new Feed { Label = false, Cluster = 3, Number = 6, Content="I'm already tired of sentences" },
				new Feed { Label = false, Cluster = 2, Number = 7, Content="3.14159 fake words flummox 8 life" },
				new Feed { Label = false, Cluster = 1, Number = 8, Content="jumbled characters ($) are funny" },
				new Feed { Label = false, Cluster = 0, Number = 9, Content="people often steal [25] dreams" },
			};
		}

		public static double[,] GetDataMatrix()
		{
			return new double[,] {																																												
				{   9,	1,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,  0 },
				{   8,	1,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	1,	0,	0,	0,	1,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  1 },
				{   7,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	1,  2 },
				{   6,	1,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,  3 },
				{   5,	0,	0,	1,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  4 },
				{   4,	1,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	1,	0,	0,  5 },
				{   3,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	1,	0,	0,	0,	0,	1,	0,	0,	0,  6 },
				{   2,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,  7 },
				{   1,	0,	0,	0,	1,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  8 },
				{   0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	1,	0,	0,	0,	0,	1,	0,	0,	0,	0,	0,	0,  9 }
			};																																												
																																										
		}
	}
}
