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
            //listView.SelectionChanged += OnCollectionViewSelectionChanged;
        }

        async void OnAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RE_EntryPage
            {
                BindingContext = new RealEstate(),
            });
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault();
            if (item != null)
            {
                await Navigation.PushAsync(new Detail (item as RealEstate));
            }
        }
    }
}