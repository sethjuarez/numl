using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public Boolean VarBoolean { get; set; }
        public Byte VarByte { get; set; }
        public SByte VarSbyte { get; set; }
        public Char VarChar { get; set; }
        public Decimal VarDecimal { get; set; }
        public Double VarDouble { get; set; }
        public Single VarSingle { get; set; }
        public Int16 VarInt16 { get; set; }
        public UInt16 VarUInt16 { get; set; }
        public Int32 VarInt32 { get; set; }
        public UInt32 VarUInt32 { get; set; }
        public Int64 VarInt64 { get; set; }
        public UInt64 VarUInt64 { get; set; }
        public Enum VarEnum { get; set; }
        public TimeSpan VarTimeSpan { get; set; }

        // default ignore types
        public Guid VarGuid { get; set; }

        // implicit multivariate types
        public String VarString { get; set; }
        public DateTime VarDateTime { get; set; }

        // explicit multivariate types
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
                    VarEnum = (Enum)FakeEnum.Item0,
                    VarTimeSpan = (TimeSpan)TimeSpan.FromSeconds(100),

                    VarString = (String)"test1",
                    VarDateTime = (DateTime)DateTime.Now,
                    
                    VarGuid = (Guid)Guid.NewGuid(),
                    VarByteArray = (Byte[])new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
                };
        }
    }
}
