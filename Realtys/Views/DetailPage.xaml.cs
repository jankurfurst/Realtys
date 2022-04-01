using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views;

public partial class DetailPage : ContentPage
{

	public DetailPage()
	{
		var viewModel = (DetailViewModel)BindingContext;
		InitializeComponent();

		//if(viewModel.mortgage == null)
  //      {
		//	mortgageCheckBox.IsVisible = false;
		//}
	}

	async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		await DisplayAlert(nameof(sender), "Realty will be shown:" + e.Value, "OK");
		testLabel1.IsVisible = e.Value;
	}

	async void OnCheckBoxCheckedChangedSecond(object sender, CheckedChangedEventArgs e)
	{
		await DisplayAlert("Mortgage will be shown", "Value:" + e.Value, "OK");
		testLabel2.IsVisible = e.Value;
	}
}