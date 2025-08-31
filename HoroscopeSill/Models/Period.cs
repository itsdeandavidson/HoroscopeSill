using System.Text.Json.Serialization;

namespace HoroscopeSill.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Period
{
	Daily,
	Weekly,
	Monthly
}