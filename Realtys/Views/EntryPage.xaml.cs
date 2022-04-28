using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {

        public EntryPage(EditCreateViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            addMortgageCheckBox.CheckedChanged -= OnCheckBoxCheckedChanged;
            addMortgageCheckBox.IsChecked = viewModel.RealEstate.MortgageUsage;
            addMortgageCheckBox.CheckedChanged += OnCheckBoxCheckedChanged;
        }

        async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            addMortgageCheckBox.CheckedChanged -= OnCheckBoxCheckedChanged;
            var viewModel = (EditCreateViewModel)BindingContext;

            if (!mortgageGrid.IsVisible)
            {
                bool check = await DisplayAlert(
                    "Přidání úvěru",
                    "Bude přidán úvěr k nemovitosti",
                    "OK",
                    "Cancel"
                    );

                if (check)
                {
                    viewModel.IsMortgageUsed = check;
                }
                addMortgageCheckBox.IsChecked = check;
            }
            else
            {
                bool check = await DisplayAlert(
                    "Odebrání úvěru", 
                    "Bude odebrán úvěr k nemovitosti, veškerý vyplněný obsah bude ztracen!", 
                    "OK", 
                    "Cancel"
                    );

                if (check)
                {
                    viewModel.IsMortgageUsed = !check;
                    viewModel.Mortgage = new Mortgage();
                }
                addMortgageCheckBox.IsChecked = !check;

            }
            addMortgageCheckBox.CheckedChanged += OnCheckBoxCheckedChanged;
        }

    }
}