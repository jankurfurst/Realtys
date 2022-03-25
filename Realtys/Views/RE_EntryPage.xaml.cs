using Realtys.Database;
using Realtys.Models;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RE_EntryPage : ContentPage
    {
        RealtysDbContext DbContext;

        public RE_EntryPage()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var re = (RealEstate)BindingContext;
            DbContext.RealEstates.Update(re);
            await Navigation.PushAsync(new RE_List());
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var re = (RealEstate)BindingContext;
            DbContext.RealEstates.Remove(re);
            await Navigation.PushAsync(new RE_List());
        }
    }
}