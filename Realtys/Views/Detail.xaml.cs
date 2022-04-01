using Realtys.Models;

namespace Realtys;

public partial class Detail : ContentPage
{
	public Detail(RealEstate realEstate)
	{
		InitializeComponent();
	}

	async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		await DisplayAlert(nameof(sender), "Value:" + e.Value, "OK");
		testLabel.IsVisible = e.Value;
	}
}