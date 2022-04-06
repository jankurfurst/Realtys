using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        private readonly RealtysDbContext DbContext;
        EntryPage EntryPage;

        public ListPage(RealtysDbContext dbContext, EntryPage entryPage)
        {
            InitializeComponent();
            DbContext = dbContext;
            EntryPage = entryPage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            var realties = DbContext.RealEstates.ToList();
            listView.ItemsSource = realties;
            listView.SelectedItem = null;
            
        }


        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault();
            if (item != null)
            {
                await Navigation.PushAsync(new DetailPage() 
                { 
                    BindingContext = new DetailViewModel((item as RealEstate).ID)
                });
            }
        }
    }
}