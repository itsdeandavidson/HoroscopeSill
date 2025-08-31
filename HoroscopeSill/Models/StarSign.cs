using System.Text.Json.Serialization;

namespace HoroscopeSill.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StarSign
{
	Aries,
	Taurus,
	Gemini,
	Cancer,
	Leo,
	Virgo,
	Libra,
	Scorpio,
	Sagittarius,
	Capricorn,
	Aquarius,
	Pisces
}