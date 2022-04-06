using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views;

public partial class DetailPage : ContentPage
{

	public DetailPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{

		var viewModel = (DetailViewModel)BindingContext;

		if (viewModel.mortgage == null || viewModel.realEstate.MortgageUsage == false)
        {
			mortgageDetail.IsVisible = false;
			mortgageCheckBox.IsVisible = false;
        }
        else
        {
			mortgageDetail.IsVisible = false;
			mortgageCheckBox.IsVisible = true;
        }

	}

	async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		await DisplayAlert(nameof(sender), "Realty will be shown:" + e.Value, "OK");
		realtyDetail.IsVisible = e.Value;
		
	}

	async void OnCheckBoxCheckedChangedSecond(object sender, CheckedChangedEventArgs e)
	{
		await DisplayAlert("Mortgage will be shown", "Value:" + e.Value, "OK");
		mortgageDetail.IsVisible = e.Value;
		
	}
}