using Realtys.Database;
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
            //var r = (RealEstate)BindingContext;
            RealEstate r = new RealEstate()
            {
                ID = 1,
                Nazev = "Novy",
                cenaNemovitosti = 55555,
                mesicniNajem = 44444,
                neobsazenost = 0
            };
            if (r != null)
            {
                var re = App.DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

                if (re != null)
                {
                    re.Nazev = r.Nazev;
                    re.cenaNemovitosti = r.cenaNemovitosti;
                    re.mesicniNaklady = r.mesicniNaklady;
                    re.mesicniNajem = r.mesicniNajem;
                    re.neobsazenost = r.neobsazenost;

                    await App.DbContext.SaveChangesAsync();
                }
                else
                {
                    App.DbContext.RealEstates.Add(r);
                    await App.DbContext.SaveChangesAsync();
                }
            }
            await Navigation.PushAsync(new RE_List());
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var r = (RealEstate)BindingContext;
            if (r != null)
            {
                var re = App.DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);

                if (re != null)
                {
                    App.DbContext.RealEstates.Remove(re);
                    await App.DbContext.SaveChangesAsync();
                }

            }
            await Navigation.PushAsync(new RE_List());
        }
    }
}