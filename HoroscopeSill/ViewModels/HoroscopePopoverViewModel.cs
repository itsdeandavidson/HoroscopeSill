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
	[NotifyPropertyChangedFor("ShowContent")]
	public partial Horoscope? Horoscope { get; set; }
	[ObservableProperty]
	public partial StarSign? StarSign { get; set; }
	[ObservableProperty]
	public partial Period? HoroscopePeriod { get; set; }
	[ObservableProperty]
	[NotifyPropertyChangedFor("ShowContent")]
	public partial string? ErrorMessage { get; set; }
	[ObservableProperty]
	public partial string DailySelection { get; set; } = "TODAY";
	public bool ShowContent => string.IsNullOrEmpty(ErrorMessage) && Horoscope is not null;

	public HoroscopePopoverViewModel(ISettingsProvider settingsProvider, Period period)
	{
		SettingsProvider = settingsProvider;
		HoroscopePeriod = period;

		SettingsProvider.SettingChanged += SettingsProvider_SettingChanged;
	}

	public async void OnOpeningAsync() => await Load();

	private async Task Load()
	{
		StarSign = SettingsProvider.GetSetting(Settings.StarSign);

		(Horoscope, ErrorMessage) = await (HoroscopePeriod switch
		{
			Period.Daily => HoroscopeService.Daily(StarSign.Value, DailySelection),
			Period.Weekly => HoroscopeService.Weekly(StarSign.Value),
			Period.Monthly => HoroscopeService.Monthly(StarSign.Value),
			_ => throw new InvalidOperationException()
		});
	}

	partial void OnDailySelectionChanged(string value) => _ = Load();

	private void SettingsProvider_SettingChanged(ISettingsProvider sender, SettingChangedEventArgs args)
	{
		if (args.SettingName == Settings.StarSign.Name && args.NewValue is not null)
			_ = Load();
	}

	void IDisposable.Dispose() => SettingsProvider.SettingChanged -= SettingsProvider_SettingChanged;
}