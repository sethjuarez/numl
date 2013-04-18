using System;
using System.Linq;
using System.Collections.Generic;
using numl.Model;

namespace numl.Tests.Data
{
    public enum FakeEnum
    {
        Item0 = 0,
        Item1 = 1,
        Item2 = 2,
        Item3 = 3,
        Item4 = 4,
        Item5 = 5,
        Item6 = 6,
        Item7 = 7,
        Item8 = 8,
        Item9 = 9
    }

    public class Generic
    {
        // univariate types
        [Feature]
        public Boolean VarBoolean { get; set; }
        [Feature]
        public Byte VarByte { get; set; }
        [Feature]
        public SByte VarSbyte { get; set; }
        [Feature]
        public Char VarChar { get; set; }
        [Feature]
        public Decimal VarDecimal { get; set; }
        [Feature]
        public Double VarDouble { get; set; }
        [Feature]
        public Single VarSingle { get; set; }
        [Feature]
        public Int16 VarInt16 { get; set; }
        [Feature]
        public UInt16 VarUInt16 { get; set; }
        [Feature]
        public Int32 VarInt32 { get; set; }
        [Feature]
        public UInt32 VarUInt32 { get; set; }
        [Feature]
        public Int64 VarInt64 { get; set; }
        [Feature]
        public UInt64 VarUInt64 { get; set; }
        [Feature]
        public FakeEnum VarEnum { get; set; }
        [Feature]
        public TimeSpan VarTimeSpan { get; set; }

        // default ignore types
        public Guid VarGuid { get; set; }

        // implicit multivariate types
        [Feature]
        public String VarString { get; set; }
        [DateFeature(DatePortion.Date | DatePortion.Time)]
        public DateTime VarDateTime { get; set; }

        // explicit multivariate types
        [EnumerableFeature(10)]
        public Byte[] VarByteArray { get; set; }

        public static IEnumerable<Generic> GetRows(int count)
        {
            while (count-- > 0)
                yield return new Generic
                {
                    VarBoolean = (Boolean)true,
                    VarByte = (Byte)0xFF,
                    VarSbyte = (SByte)1,
                    VarChar = (Char)'A',
                    VarDecimal = (Decimal)0.4m,
                    VarDouble = (Double)0.1d,
                    VarSingle = (Single)300f,
                    VarInt16 = (Int16)1,
                    VarUInt16 = (UInt16)2,
                    VarInt32 = (Int32)3,
                    VarUInt32 = (UInt32)4,
                    VarInt64 = (Int64)5,
                    VarUInt64 = (UInt64)6,
                    VarEnum = FakeEnum.Item0,
                    VarTimeSpan = (TimeSpan)TimeSpan.FromSeconds(100),

                    VarString = (String)"test1",
                    VarDateTime = (DateTime)DateTime.Now,
                    
                    VarGuid = (Guid)Guid.NewGuid(),
                    VarByteArray = (Byte[])new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
                };
        }
    }
}
