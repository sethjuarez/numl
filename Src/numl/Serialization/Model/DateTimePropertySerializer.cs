﻿using System;
using numl.Model;
using System.Linq;
using System.Collections.Generic;

namespace numl.Serialization.Model
{
    public class DateTimePropertySerializer : numl.Serialization.Model.PropertySerializer
    {
        
        public override bool CanConvert(Type type)
        {
            return typeof(DateTimeProperty).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new DateTimeProperty();
        }

        public override object Read(JsonReader reader)
        {
            var p = (DateTimeProperty)base.Read(reader);

            var features = ((object[])reader.ReadArrayProperty().Value)
                            .Select(o => (string)o)
                            .ToArray();

            p.Features = DateTimeProperty.GetFeatures(features);

            return p;
        }

        public override void Write(JsonWriter writer, object value)
        {
            base.Write(writer, value);
            var p = (DateTimeProperty)value;
            writer.WriteFirstArrayProperty("Features", DateTimeProperty.GetColumns(p.Features));
        }
        
    }
}