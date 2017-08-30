using numl.Model;
using System;
using Xunit;

namespace numl.Tests.SerializationTests.DescriptorSerialization
{
	[Trait("Category", "Serialization")]
	public class PropertySerializationTests : BaseSerialization
	{
		[Fact]
		public void Standard_Property_Save_And_Load()
		{
			Property p = new Property();
			p.Name = "MyProp";
			p.Type = typeof(decimal);
			p.Discrete = false;
			p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoad(p, file);
			Assert.Equal(p, property);
		}

        [Fact]
        public void Standard_Property_Save_And_Load_Json()
        {
            Property p = new Property();
            p.Name = "MyProp";
            p.Type = typeof(decimal);
            p.Discrete = false;
            p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoadJson(p);
            Assert.Equal(p, property);
        }

        [Fact]
		public void String_Property_Save_And_Load()
		{
			StringProperty p = new StringProperty();
			p.Name = "MyProp";
			p.Start = 5;

			p.Dictionary = new string[] { "ONE", "TWO", "THREE", "FOUR" };
			p.Exclude = new string[] { "FIVE", "SIX", "SEVEN" };

            var file = GetPath();
            var property = SaveAndLoad(p, file);
            Assert.Equal(p, property);
        }

        [Fact]
        public void String_Property_Save_And_Load_Json()
        {
            StringProperty p = new StringProperty();
            p.Name = "MyProp";
            p.Start = 5;

            p.Dictionary = new string[] { "ONE", "TWO", "THREE", "FOUR" };
            p.Exclude = new string[] { "FIVE", "SIX", "SEVEN" };

            var file = GetPath();
            var property = SaveAndLoadJson(p);
            Assert.Equal(p, property);
        }

        [Fact]
		public void Enumerable_Property_Save_And_Load()
		{
			EnumerableProperty p = new EnumerableProperty(100);
			p.Name = "MyProp";
			p.Type = typeof(decimal);
			p.Discrete = false;
			p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoad(p, file);
            Assert.Equal(p, property);
        }

        [Fact]
        public void Enumerable_Property_Save_And_Load_Json()
        {
            EnumerableProperty p = new EnumerableProperty(100);
            p.Name = "MyProp";
            p.Type = typeof(decimal);
            p.Discrete = false;
            p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoadJson(p);
            Assert.Equal(p, property);
        }

        [Fact]
		public void DateTime_Property_Save_And_Load()
		{
			DateTimeProperty p =
				new DateTimeProperty(DateTimeFeature.DayOfWeek |
									 DateTimeFeature.Second |
									 DateTimeFeature.Minute);
			p.Name = "MyProp";
			p.Discrete = false;
			p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoad(p, file);
            Assert.Equal(p, property);
        }

        [Fact]
        public void DateTime_Property_Save_And_Load_Json()
        {
            DateTimeProperty p =
                new DateTimeProperty(DateTimeFeature.DayOfWeek |
                                     DateTimeFeature.Second |
                                     DateTimeFeature.Minute);
            p.Name = "MyProp";
            p.Discrete = false;
            p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoadJson(p);
            Assert.Equal(p, property);
        }

        [Fact]
		public void Guid_Property_Save_And_Load()
		{
			GuidProperty p = new GuidProperty();
			p.Name = "MyProp";
			p.Categories = new Guid[] { Guid.NewGuid() };
			p.Discrete = false;
			p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoad(p, file);
            Assert.Equal(p, property);
        }

        [Fact]
        public void Guid_Property_Save_And_Load_Json()
        {
            GuidProperty p = new GuidProperty();
            p.Name = "MyProp";
            p.Categories = new Guid[] { Guid.NewGuid() };
            p.Discrete = false;
            p.Start = 5;

            var file = GetPath();
            var property = SaveAndLoadJson(p);
            Assert.Equal(p, property);
        }
    }
}
