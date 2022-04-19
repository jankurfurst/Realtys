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

		//if (viewModel.mortgage == null || viewModel.realEstate.MortgageUsage == false)
  //      {
		//	mortgageDetail.IsVisible = false;
  //      }
  //      else
  //      {
		//	mortgageDetail.IsVisible = true;
  //      }

	}

}