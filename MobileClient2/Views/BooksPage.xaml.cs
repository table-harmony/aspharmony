using MobileClient2.ViewModels;

namespace MobileClient2.Views;

public partial class BooksPage : ContentPage
{
	private readonly BooksViewModel _viewModel;

	public BooksPage(BooksViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadBooksAsync();
	}

	private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
	{
		_viewModel.FilterBooks(e.NewTextValue);
	}
}