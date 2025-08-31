using CommunityToolkit.WinUI.Controls;
using HoroscopeSill.Models;
using HoroscopeSill.ViewModels;
using WindowSill.API;

namespace HoroscopeSill.Views;

public class SettingsView : UserControl
{
	public SettingsView(ISettingsProvider settingsProvider) => this.DataContext(
		new SettingsViewModel(settingsProvider),
		(view, viewModel) => view
			.Content(
				new StackPanel()
					.Spacing(2)
					.Children(
						new TextBlock()
							.Style(x => x.ThemeResource("BodyStrongTextBlockStyle"))
							.Margin(0, 0, 0, 8)
							.Text("/HoroscopeSill/Resources/Settings_SectionHeader".GetLocalizedString()),

						new SettingsCard()
							.Header("/HoroscopeSill/Resources/Settings_Card_StarSign_Header".GetLocalizedString())
							.Description("/HoroscopeSill/Resources/Settings_Card_StarSign_Description".GetLocalizedString())
							.HeaderIcon(new FontIcon().Glyph("\uE734"))
							.Content(
								new ComboBox()
									.ItemsSource(Enum.GetValues(typeof(StarSign)))
									.MaxDropDownHeight(250)
									.SelectedItem(x => x
										.Binding(() => viewModel.StarSign)
										.TwoWay()
										.UpdateSourceTrigger(UpdateSourceTrigger.PropertyChanged)
									)
							)
					)
			)
	);
}