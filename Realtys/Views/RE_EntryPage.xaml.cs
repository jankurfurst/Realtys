using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RE_EntryPage : ContentPage
    {
        private readonly RealtysDbContext DbContext;

        public RE_EntryPage(RealtysDbContext dbContext, EditViewModel viewModel)
        {
            InitializeComponent();
            DbContext = dbContext;
            BindingContext = viewModel;
            ((EditViewModel)BindingContext).RealEstate = new RealEstate();
            ((EditViewModel)BindingContext).Mortgage = new Mortgage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (EditViewModel)BindingContext;
            viewModel.RealEstate = new RealEstate();
            viewModel.Mortgage = new Mortgage();
            
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

            if (r != null)
            {
                var re = DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

                if (re != null)
                {
                    re.Nazev = r.Nazev;
                    re.cenaNemovitosti = r.cenaNemovitosti;
                    re.mesicniNaklady = r.mesicniNaklady;
                    re.mesicniNajem = r.mesicniNajem;
                    re.neobsazenost = r.neobsazenost;

                    await DbContext.SaveChangesAsync();
                }
                else
                {
                    DbContext.RealEstates.Add(r);
                    await DbContext.SaveChangesAsync();
                }
            }
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