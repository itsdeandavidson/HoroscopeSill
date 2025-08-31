using CommunityToolkit.Mvvm.ComponentModel;
using HoroscopeSill.Models;
using HoroscopeSill.Services.Services;
using WindowSill.API;

namespace HoroscopeSill.ViewModels;

public sealed partial class HoroscopePopoverViewModel : ObservableObject, IDisposable
{
	private readonly HoroscopeService HoroscopeService = new();
	private readonly ISettingsProvider SettingsProvider;
	[ObservableProperty]
	public partial Horoscope? Horoscope { get; set; }
	[ObservableProperty]
	public partial StarSign StarSign { get; set; } = StarSign.Aries;
	[ObservableProperty]
	public partial Period? HoroscopePeriod { get; set; }
	[ObservableProperty]
	public partial string? ErrorMessage { get; set; }
	[ObservableProperty]
	public partial string DailySelection { get; set; } = "TODAY";

	public HoroscopePopoverViewModel(ISettingsProvider settingsProvider, Period period)
	{
		SettingsProvider = settingsProvider;
		HoroscopePeriod = period;
		_ = Load();
	}

	private async Task Load()
	{
		StarSign = SettingsProvider.GetSetting(Settings.StarSign);

		SettingsProvider.SettingChanged += SettingsProvider_SettingChanged;

		(Horoscope, ErrorMessage) = await (HoroscopePeriod switch
		{
			Period.Daily => HoroscopeService.Daily(StarSign, DailySelection),
			Period.Weekly => HoroscopeService.Weekly(StarSign),
			Period.Monthly => HoroscopeService.Monthly(StarSign),
			_ => throw new InvalidOperationException()
		});
	}

	partial void OnDailySelectionChanged(string value) => _ = Load();

	private void SettingsProvider_SettingChanged(ISettingsProvider sender, SettingChangedEventArgs args)
	{
		if (args.SettingName == nameof(Settings.StarSign) && args.NewValue is not null)
			StarSign = (StarSign)args.NewValue;

		_ = Load();
	}

	void IDisposable.Dispose() => SettingsProvider.SettingChanged -= SettingsProvider_SettingChanged;

}