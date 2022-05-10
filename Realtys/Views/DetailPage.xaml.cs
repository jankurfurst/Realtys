using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views;

public partial class DetailPage : ContentPage
{

	public DetailPage()
	{
		InitializeComponent();

		var platform = DeviceInfo.Platform;

		if(platform == DevicePlatform.MacCatalyst || platform == DevicePlatform.WinUI)
        {
			priceSlider.Interval = 200000;

			rentSlider.Interval = 10000;

			expensesSlider.Interval = 2000;

			interestSlider.MinorTicksPerInterval = 10;
			interestSlider.Interval = 1;
		}
		else if(platform == DevicePlatform.Android || platform == DevicePlatform.iOS)
        {
			priceSlider.Interval = 500000;

			rentSlider.Interval = 50000;

			expensesSlider.Interval = 10000;

			interestSlider.MinorTicksPerInterval = 4;
			interestSlider.Interval = 5;
		}
	}

}