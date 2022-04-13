using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {
        private readonly RealtysDbContext DbContext;

        public EntryPage(RealtysDbContext dbContext, EditCreateViewModel viewModel)
        {
            InitializeComponent();
            DbContext = dbContext;
            BindingContext = viewModel;
            addMortgageCheckBox.IsChecked = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((EditCreateViewModel)BindingContext).RealEstate = new RealEstate();
            ((EditCreateViewModel)BindingContext).Mortgage = new Mortgage();
            addMortgageCheckBox.IsChecked = false;
        }

        async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
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
                    addMortgageCheckBox.IsChecked = viewModel.IsMortgageUsed = check;
                }
            }
            else
            {
                bool check = await DisplayAlert(
                    "Odebrání úvěru", 
                    "Bude odebrán úvěr k nemovitosti, veškerý vyplněný obsah, bude ztracen!", 
                    "OK", 
                    "Cancel"
                    );

                if (check)
                {
                    addMortgageCheckBox.IsChecked = viewModel.IsMortgageUsed = !check;
                    ((EditCreateViewModel)BindingContext).Mortgage = new Mortgage();
                }

            }
            await Shell.Current.GoToAsync("//Entry");
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var status = await DisplayAlert("Item Object", "Title property:" + nameof(OnSaveButtonClicked), "OK", "Cancel");
            if (!status) return;

            var viewModel = (EditCreateViewModel)BindingContext;
            if (viewModel.SaveCommand.CanExecute(null))
                viewModel.SaveCommand.Execute(null);

            
            //var r = ((EditCreateViewModel)BindingContext).RealEstate;
            //var m = ((EditCreateViewModel)BindingContext).Mortgage;

            //if (r != null)
            //{
            //    var re = DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

            //    if (re != null)
            //    {
            //        re.Name = r.Name;
            //        re.RealtyPrice = r.RealtyPrice;
            //        re.MonthlyExpenses = r.MonthlyExpenses;
            //        re.MonthlyRent = r.MonthlyRent;
            //        re.Vacancy = r.Vacancy;
            //        re.MortgageUsage = addMortgageCheckBox.IsChecked;

            //        await DbContext.SaveChangesAsync();
            //    }
            //    else
            //    {
            //        try
            //        {
            //            DbContext.RealEstates.Add(r);
            //            await DbContext.SaveChangesAsync();
            //        }
            //        catch(Exception ex)
            //        {
            //            await DisplayAlert("Error while saving", "Missing parametr: " + ex.InnerException.Message, "Cancel");
            //        }
            //    }
            //}

            //m.RealtyID = r.ID;
            //if (addMortgageCheckBox.IsChecked == true)
            //{
            //    var mortgage = DbContext.Mortgages.FirstOrDefault(_m => _m.ID == m.ID);
            //    if (mortgage != null)
            //    {
            //        mortgage.Share = m.Share;
            //        mortgage.RealtyID = r.ID;

            //        await DbContext.SaveChangesAsync();
            //    }
            //    else
            //    {
            //        try
            //        {
            //            DbContext.Mortgages.Add(m);
            //            await DbContext.SaveChangesAsync();
            //        }
            //        catch (Exception ex)
            //        {
            //            await DisplayAlert("Error while saving", "Missing parametr: " + ex.InnerException.Message, "Cancel");
            //        }
            //    }
            //}
            ////Realty entries
            //nameEntry.Text = priceEntry.Text = rentEntry.Text = 
            //    expensesEntry.Text = vacancyEntry.Text = string.Empty;

            ////Mortgage entries
            //interestEntry.Text = shareEntry.Text = forYearsEntry.Text = string.Empty;

            //await Shell.Current.GoToAsync("//first");
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            ((EditCreateViewModel)BindingContext).SaveCommand.Execute(null);
            var r = ((EditCreateViewModel)BindingContext).RealEstate;
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