using Realtys.Models;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RE_EntryPage : ContentPage
    {

        public RE_EntryPage()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var r = (RealEstate)BindingContext;

            var re = App.DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

            if (re != null)
            {
                App.DbContext.RealEstates.Update(re);
            }
            await Navigation.PushAsync(new RE_List());
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var r = (RealEstate)BindingContext;

            var re = App.DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

            if(re != null)
            {
                App.DbContext.RealEstates.Remove(re);
            }
            await Navigation.PushAsync(new RE_List());
        }
    }
}