using Realtys.Database;
using Realtys.Models;



namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RE_List : ContentPage
    {


        public RE_List()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //listView.ItemsSource = await App.Database.GetREs_Async();
            var realties = App.DbContext.RealEstates.ToList();
            listView.ItemsSource = realties;
        }

        async void OnAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RE_EntryPage
            {
                BindingContext = new RealEstate(),
            });
        }

        async void OnCollectionViewItemSelected(object sender, SelectedItemChangedEventArgs e)
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