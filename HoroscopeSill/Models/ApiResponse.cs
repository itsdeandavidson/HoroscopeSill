using System.Text.Json.Serialization;

namespace HoroscopeSill.Models;

public class ApiResponse<T>
{
	[JsonPropertyName("data")]
	public T? Data { get; set; }

	[JsonPropertyName("status")]
	public int Status { get; set; }

	[JsonPropertyName("success")]
	public bool Success { get; set; }
}