using HoroscopeSill.Models;

namespace HoroscopeSill.Services.Interfaces;

public interface IHoroscopeService
{
	Task<(Horoscope? Horoscope, string? ErrorMessage)> Daily(StarSign sign, string day = "TODAY");
	Task<(Horoscope? Horoscope, string? ErrorMessage)> Weekly(StarSign sign);
	Task<(Horoscope? Horoscope, string? ErrorMessage)> Monthly(StarSign sign);
}