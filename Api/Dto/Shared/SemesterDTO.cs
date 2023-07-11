namespace Api.Dto.Shared
{
    namespace plms.ManagementService.Model.DTO
    {
    public class SemesterDTO
    {
            public string Code { get; set; }

            [JsonConverter(typeof(CustomDateTimeConverter))]
            public DateTime StartDate { get; set; }

            [JsonConverter(typeof(CustomDateTimeConverter))]
            public DateTime EndDate { get; set; }
        }

        public class CustomDateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (DateTime.TryParseExact(reader.GetString(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                return DateTime.MinValue;
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
            }
        }
    }
}
