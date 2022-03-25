using Realtys.Database;
using Realtys.Models;



namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RE_List : ContentPage
    {

        readonly RealtysDbContext DbContext;

        public RE_List()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //listView.ItemsSource = await App.Database.GetREs_Async();
            listView.ItemsSource = DbContext.RealEstates.ToList();
        }

        async void OnREAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RE_EntryPage
            {
                BindingContext = new RealEstate()
            });
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new RE_EntryPage
                {
                    BindingContext = e.SelectedItem as RealEstate
                });
            }
        }
    }
}