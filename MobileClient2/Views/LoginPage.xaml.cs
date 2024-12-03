using MobileClient2.ViewModels;

namespace MobileClient2.Views;

public partial class LoginPage : ContentPage {
	private readonly LoginViewModel _viewModel;

	public LoginPage(LoginViewModel viewModel) {
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override void OnAppearing() {
		base.OnAppearing();
		_viewModel.Email = string.Empty;
		_viewModel.Password = string.Empty;
	}
}