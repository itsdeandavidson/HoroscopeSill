using WindowSill.API;

namespace HoroscopeSill.Models;

public class Settings
{
	public static readonly SettingDefinition<StarSign> StarSign = new(Models.StarSign.Aries, typeof(Settings).Assembly);
}
