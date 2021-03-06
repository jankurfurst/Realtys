using Realtys.Database;
using Realtys.Models;
using Realtys.ViewModels;

namespace Realtys.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        private readonly RealtysDbContext DbContext;
 

        public ListPage(RealtysDbContext dbContext)
        {
            InitializeComponent();
            DbContext = dbContext;
            listView.SelectionChanged += OnCollectionViewSelectionChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            var realties = DbContext.RealEstates.ToList();
            realties.Reverse();
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