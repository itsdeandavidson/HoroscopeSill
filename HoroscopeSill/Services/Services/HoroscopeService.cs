using HoroscopeSill.Models;
using HoroscopeSill.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace HoroscopeSill.Services.Services;

public class HoroscopeService : IHoroscopeService, IDisposable
{
	private readonly string ApiBase = "https://horoscope-app-api.vercel.app/api/v1/get-horoscope";
	private readonly HttpClient HttpClient;

	public HoroscopeService()
	{
		HttpClient = new HttpClient();
	}

	public async Task<(Horoscope? Horoscope, string? ErrorMessage)> Daily(StarSign sign, string day = "TODAY")
	{
		string[] allowedDays = ["TODAY", "TOMORROW", "YESTERDAY"];

		if (!allowedDays.Contains(day.ToUpper()))
		{
			if (DateTime.TryParse(day, out var date))
			{
				day = date.ToString("yyyy-MM-dd");
			}
			else
			{
				return (null, "Day must be 'TODAY', 'TOMORROW', 'YESTERDAY', or a valid date.");
			}
		}

		return await Get($"{ApiBase}/daily?sign={sign}&day={day.ToUpper()}");
	}

	public async Task<(Horoscope? Horoscope, string? ErrorMessage)> Weekly(StarSign sign) => await Get($"{ApiBase}/weekly?sign={sign}");

	public async Task<(Horoscope? Horoscope, string? ErrorMessage)> Monthly(StarSign sign) => await Get($"{ApiBase}/monthly?sign={sign}");

	private async Task<(Horoscope? Horoscope, string? ErrorMessage)> Get(string url)
	{
		try
		{
			HttpResponseMessage response = await HttpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
				return (null, $"HTTP error {(int)response.StatusCode}: {response.ReasonPhrase}");

			ApiResponse<Horoscope>? apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<Horoscope>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return apiResponse?.Data is not null && apiResponse.Success ? (apiResponse.Data, null) : (null, "Response content was empty or invalid.");
		}
		catch (Exception ex)
		{
			return (null, ex.Message);
		}
	}

	public void Dispose() => HttpClient.Dispose();
}