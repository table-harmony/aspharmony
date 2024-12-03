using MobileClient2.ViewModels;

namespace MobileClient2.Views;

public partial class LibrariesPage : ContentPage
{
	private readonly LibrariesViewModel _viewModel;

	public LibrariesPage(LibrariesViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadLibrariesAsync();
	}

	private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
	{
		_viewModel.FilterLibraries(e.NewTextValue);
	}
}