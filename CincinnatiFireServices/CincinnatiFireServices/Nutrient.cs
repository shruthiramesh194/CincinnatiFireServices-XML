﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using NutrientQuickType;
//
//    var nutrient = Nutrient.FromJson(jsonString);

namespace NutrientQuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Nutrient
    {
        [JsonProperty("fdcId")]
        public long FdcId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("foodNutrients")]
        public FoodNutrient[] FoodNutrients { get; set; }

        [JsonProperty("brandedFoodCategory")]
        public string BrandedFoodCategory { get; set; }

        [JsonProperty("servingSize")]
        public long ServingSize { get; set; }

        [JsonProperty("servingSizeUnit")]
        public string ServingSizeUnit { get; set; }

        [JsonProperty("portionSize")]
        public long PortionSize { get; set; }

        [JsonProperty("foodCategory")]
        public object FoodCategory { get; set; }
    }

    public partial class FoodNutrient
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unitName")]
        public string UnitName { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }

    public partial class Nutrient
    {
        public static Nutrient[] FromJson(string json) => JsonConvert.DeserializeObject<Nutrient[]>(json, NutrientQuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Nutrient[] self) => JsonConvert.SerializeObject(self, NutrientQuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
