using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {
        private readonly RealtysDbContext DbContext;

        public EntryPage(RealtysDbContext dbContext, EditViewModel viewModel)
        {
            InitializeComponent();
            DbContext = dbContext;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((EditViewModel)BindingContext).RealEstate = new RealEstate();
            ((EditViewModel)BindingContext).Mortgage = new Mortgage();

        }

        async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!mortgageGrid.IsVisible)
            {
                await DisplayAlert("Přidání úvěru", "Bude přidán úvěr k nemovitosti", "OK");
                mortgageGrid.IsVisible = e.Value;
                await Shell.Current.GoToAsync("//Entry");
            }
            else
            {
                var check = await DisplayAlert("Odebrání úvěru", "Bude odebrán úvěr k nemovitosti, veškerý vyplněný obsah, bude ztracen!", "OK", "Cancel");
                if (check == true)
                {
                    mortgageGrid.IsVisible = e.Value;
                    await Shell.Current.GoToAsync("//Entry");

                }
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var status = await DisplayAlert("Item Object", "Title property:" + nameof(OnSaveButtonClicked), "OK", "Cancel");
            if (!status) return;

            var r = ((EditViewModel)BindingContext).RealEstate;
            var m = ((EditViewModel)BindingContext).Mortgage;

            if (r != null)
            {
                var re = DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

                if (re != null)
                {
                    re.Name = r.Name;
                    re.RealtyPrice = r.RealtyPrice;
                    re.MonthlyExpenses = r.MonthlyExpenses;
                    re.MonthlyRent = r.MonthlyRent;
                    re.Vacancy = r.Vacancy;

                    await DbContext.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        DbContext.RealEstates.Add(r);
                        await DbContext.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        await DisplayAlert("Error while saving", "Missing parametr: " + ex.InnerException.Message, "Cancel");
                    }
                }
            }
            if (addMortgageCheckBox.IsChecked == true )
            {
                var check = addMortgageCheckBox.IsChecked;
            }
            //Realty entries
            nameEntry.Text = priceEntry.Text = rentEntry.Text = 
                expensesEntry.Text = vacancyEntry.Text = string.Empty;

            //Mortgage entries
            interestEntry.Text = shareEntry.Text = forYearsEntry.Text = string.Empty;

            await Shell.Current.GoToAsync("//first");
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var r = (RealEstate)BindingContext;
            if (r != null)
            {
                var re = DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

                if (re != null)
                {
                    DbContext.RealEstates.Remove(re);
                    await DbContext.SaveChangesAsync();
                }

            }
            await Shell.Current.GoToAsync("//first");
        }
    }
}