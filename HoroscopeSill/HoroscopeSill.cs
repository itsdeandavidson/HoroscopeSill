using HoroscopeSill.Models;
using HoroscopeSill.Views;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using WindowSill.API;

namespace HoroscopeSill;

[Export(typeof(ISill))]
[Name("Horoscope")]
[Priority(Priority.Low)]
public sealed class HoroscopeSill : ISillActivatedByDefault, ISillListView
{
	private readonly ISettingsProvider SettingsProvider;

	[ImportingConstructor]
	internal HoroscopeSill(ISettingsProvider settingsProvider)
	{
		SettingsProvider = settingsProvider;
	}

	public string DisplayName => "/HoroscopeSill/Resources/DisplayName".GetLocalizedString();

	public IconElement CreateIcon() => new FontIcon().Glyph("\uE734");

	public SillSettingsView[]? SettingsViews => [new SillSettingsView(DisplayName, new(() => new SettingsView(SettingsProvider)))];

	public ObservableCollection<SillListViewItem> ViewList { get; } = new();

	public SillView? PlaceholderView => null;

	public async ValueTask OnActivatedAsync()
	{
		await ThreadHelper.RunOnUIThreadAsync(() =>
		{
			ViewList.Add(new SillListViewPopupItem("/HoroscopeSill/Resources/PopoverButton_Daily".GetLocalizedString(), null, HoroscopePopoverView.CreateView(SettingsProvider, Period.Daily)));
			ViewList.Add(new SillListViewPopupItem("/HoroscopeSill/Resources/PopoverButton_Weekly".GetLocalizedString(), null, HoroscopePopoverView.CreateView(SettingsProvider, Period.Weekly)));
			ViewList.Add(new SillListViewPopupItem("/HoroscopeSill/Resources/PopoverButton_Monthly".GetLocalizedString(), null, HoroscopePopoverView.CreateView(SettingsProvider, Period.Monthly)));
		});
	}

	public ValueTask OnDeactivatedAsync() => ValueTask.CompletedTask;
}