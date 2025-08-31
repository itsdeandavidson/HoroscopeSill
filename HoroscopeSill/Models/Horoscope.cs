using System.Text.Json;
using System.Text.Json.Serialization;

namespace HoroscopeSill.Models;

public class Horoscope
{
	[JsonPropertyName("horoscope_data")]
	public string? Reading { get; set; }

	[JsonPropertyName("date")]
	public string? Date { get; set; }

	[JsonPropertyName("week")]
	public string? Week { get; set; }

	[JsonPropertyName("month")]
	public string? Month { get; set; }

	[JsonPropertyName("standout_days")]
	[JsonConverter(typeof(CsvToIntArrayConverter))]
	public int[]? StandoutDayNumbers { get; set; }

	[JsonPropertyName("challenging_days")]
	[JsonConverter(typeof(CsvToIntArrayConverter))]
	public int[]? ChallengingDayNumbers { get; set; }
}

public class CsvToIntArrayConverter : JsonConverter<int[]?>
{
	public override int[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string? str = reader.GetString();
		return string.IsNullOrEmpty(str) ? null : [.. str.Split(',').Select(s => s.Trim()).Where(s => int.TryParse(s, out _)).Select(int.Parse)];
	}

	public override void Write(Utf8JsonWriter writer, int[]? value, JsonSerializerOptions options) => throw new NotImplementedException();
}