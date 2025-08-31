using CommunityToolkit.Mvvm.ComponentModel;
using HoroscopeSill.Models;
using HoroscopeSill.ViewModels;
using Microsoft.UI.Text;
using WindowSill.API;

namespace HoroscopeSill.Views;

public sealed partial class HoroscopePopoverView() : ObservableObject
{
	public static SillPopupContent CreateView(ISettingsProvider SettingsProvider, Period period)
	{
		return new SillPopupContent()
			.Width(600)
			.Height(300)
			.DataContext(
				new HoroscopePopoverViewModel(SettingsProvider, period),
				(view, viewModel) => view.Content(
					 new Grid()
						.Padding(12)
						.RowDefinitions(
							new RowDefinition() { Height = GridLength.Auto },
							new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }
						)
						.RowSpacing(12)
						.Children(
							new StackPanel()
								.Grid(row: 0)
								.Spacing(12)
								.Children(
									new InfoBar()
										.Severity(InfoBarSeverity.Error)
										.IsOpen(x => x
											.Binding(() => viewModel.ErrorMessage)
											.Convert(msg => !string.IsNullOrEmpty(msg))
										)
										.IsClosable(false)
										.Message(x => x.Binding(() => viewModel.ErrorMessage)),

									new Grid()
										.ColumnDefinitions("*,Auto")
										.Children(
											new TextBlock()
												.Grid(column: 0)
												.Text(x => x
													.Binding(() => viewModel.Horoscope)
													.Convert(h => h?.Date ?? h?.Week ?? h?.Month ?? string.Empty)
												)
												.FontSize(24)
												.FontWeight(FontWeights.SemiBold)
												.Foreground(x => x.ThemeResource("TextFillColorPrimaryBrush")),

											new ComboBox()
												.Grid(column: 1)
												.Items(new[] { "YESTERDAY", "TODAY", "TOMORROW" })
												.Width(140)
												.HorizontalAlignment(HorizontalAlignment.Right)
												.SelectedItem(x => x
													.Binding(() => viewModel.DailySelection)
													.TwoWay()
													.UpdateSourceTrigger(UpdateSourceTrigger.PropertyChanged)
												)
												.Visibility(x => x
													.Binding(() => viewModel.HoroscopePeriod)
													.Convert(period => period == Period.Daily ? Visibility.Visible : Visibility.Collapsed)
												)
										)
										.Visibility(x => x
											.Binding(() => viewModel.ErrorMessage)
											.Convert(error => string.IsNullOrWhiteSpace(error) ? Visibility.Visible : Visibility.Collapsed)
										),

										new Grid()
											.ColumnDefinitions("*,*")
											.ColumnSpacing(24)
											.Children(
												new StackPanel()
													.Grid(column: 0)
													.Spacing(6)
													.Children(
														new TextBlock().Text("/HoroscopeSill/Resources/PopoverView_StandoutDays".GetLocalizedString()).FontWeight(FontWeights.SemiBold),
														new ItemsRepeater()
															.ItemsSource(x => x.Binding(() => viewModel.Horoscope?.StandoutDayNumbers))
															.Layout(new UniformGridLayout
															{
																MinItemWidth = 56,
																MinItemHeight = 36,
																MinColumnSpacing = 8,
																MinRowSpacing = 8
															})
															.ItemTemplate(() =>
																new Border()
																	.CornerRadius(16)
																	.Background(x => x.ThemeResource("SolidBackgroundFillColorSecondaryBrush"))
																	.Padding(12, 6)
																	.Child(
																		new TextBlock()
																			.Text(x => x.Binding())
																			.FontWeight(FontWeights.SemiBold)
																			.Foreground(x => x.ThemeResource("TextFillColorPrimaryBrush"))
																			.HorizontalAlignment(HorizontalAlignment.Center)
																			.VerticalAlignment(VerticalAlignment.Center)
																	)
															)
													),

												new StackPanel()
													.Grid(column: 1)
													.Spacing(6)
													.Children(
														new TextBlock().Text("/HoroscopeSill/Resources/PopoverView_ChallengingDays".GetLocalizedString()).FontWeight(FontWeights.SemiBold),
														new ItemsRepeater()
															.ItemsSource(x => x.Binding(() => viewModel.Horoscope?.ChallengingDayNumbers))
															.Layout(new UniformGridLayout
															{
																MinItemWidth = 56,
																MinItemHeight = 36,
																MinColumnSpacing = 8,
																MinRowSpacing = 8
															})
															.ItemTemplate(() =>
																new Border()
																	.CornerRadius(16)
																	.Background(x => x.ThemeResource("SolidBackgroundFillColorSecondaryBrush"))
																	.Padding(12, 6)
																	.Child(
																		new TextBlock()
																			.Text(x => x.Binding())
																			.FontWeight(FontWeights.SemiBold)
																			.Foreground(x => x.ThemeResource("TextFillColorPrimaryBrush"))
																			.HorizontalAlignment(HorizontalAlignment.Center)
																			.VerticalAlignment(VerticalAlignment.Center)
																	)
															)
													)
											)
											.Visibility(x => x
												.Binding(() => viewModel.HoroscopePeriod)
												.Convert(period => period == Period.Monthly ? Visibility.Visible : Visibility.Collapsed)
											)

								),

								new ScrollViewer()
									.Grid(row: 1)
									.VerticalScrollBarVisibility(ScrollBarVisibility.Auto)
									.HorizontalScrollBarVisibility(ScrollBarVisibility.Disabled)
									.Content(
										new TextBlock()
											.Text(x => x
												.Binding(() => viewModel.Horoscope?.Reading)
											)
											.TextWrapping(TextWrapping.Wrap)
											.LineHeight(22)
											.Foreground(x => x.ThemeResource("TextFillColorSecondaryBrush"))
									)
									.Visibility(x => x
										.Binding(() => viewModel.ErrorMessage)
										.Convert(error => string.IsNullOrWhiteSpace(error) ? Visibility.Visible : Visibility.Collapsed)
									)
						)
				)
			);
	}
}

