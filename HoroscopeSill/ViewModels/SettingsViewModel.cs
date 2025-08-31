using CommunityToolkit.Mvvm.ComponentModel;
using HoroscopeSill.Models;
using WindowSill.API;

namespace HoroscopeSill.ViewModels;

public class SettingsViewModel(ISettingsProvider SettingsProvider) : ObservableObject
{
	public StarSign StarSign
	{
		get => SettingsProvider.GetSetting(Settings.StarSign);
		set => SettingsProvider.SetSetting(Settings.StarSign, value);
	}
}