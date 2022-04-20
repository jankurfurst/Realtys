using Realtys.Models;
using Realtys.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtys.ViewModels
{
    public class DetailViewModel
    {
        private Command _DeleteCommand;
        private Command _EditCommand;

        public RealEstate realEstate { get; set; }
        public Mortgage mortgage { get; set; }

        public Command DeleteCommand => _DeleteCommand ??= new Command(ExecuteDeleteCommand);

        [Obsolete]
        public Command EditCommand => _EditCommand ??= new Command(ExecuteEditCommand);


        public DetailViewModel(int id)
        {
            realEstate = App.DbContext.RealEstates.FirstOrDefault(r => r.ID == id);
            mortgage = App.DbContext.Mortgages.FirstOrDefault(m => m.RealtyID == id);
        }

        [Obsolete]
        private async void ExecuteEditCommand()
        {
            var status = await Shell.Current.DisplayAlert("Editace záznamu", $"Přejete si upravit záznam {this.realEstate.Name}?", "Upravit", "Zrušit");
            if (!status) return;

            var viewModel = new EditCreateViewModel()
            {
                RealEstate = realEstate,
                Mortgage = mortgage,
                IsMortgageUsed = realEstate.MortgageUsage
            };
            await Shell.Current.Navigation.PushAsync(new EntryPage(viewModel));


        }

        private async void ExecuteDeleteCommand()
        {
            var status = await Shell.Current.DisplayAlert("Odstranění záznamu", $"Přejete si aby byl záznam {this.realEstate.Name} odstraněn?", "Odstranit", "Zrušit");
            if (!status) return;

            var r = this.realEstate;
            if (r != null)
            {
                var re = App.DbContext.RealEstates.FirstOrDefault(rs => rs.ID == r.ID);
                var m = App.DbContext.Mortgages.FirstOrDefault(M => M.RealtyID == r.ID);

                if (re != null)
                {
                    App.DbContext.RealEstates.Remove(re);
                    await App.DbContext.SaveChangesAsync();
                }
                if (m != null)
                {
                    App.DbContext.Mortgages.Remove(m);
                    await App.DbContext.SaveChangesAsync();
                }

            }
            await Shell.Current.GoToAsync("//first");
        }


        private int mesicniNakladySHypo
        {
            get
            {
                if (mortgage != null) return (int)mortgage.Payment;
                return 0;                
            }
        }
        
        public int cistyRocniVynos => (int)((realEstate.MonthlyRent - mesicniNakladySHypo - realEstate.MonthlyExpenses) * (12 - realEstate.Vacancy));
        
        public int porizovaciCena
        {
            get
            {
                if (mortgage == null) return (int)realEstate.RealtyPrice;
                return (int)(realEstate.RealtyPrice - Math.Floor(mortgage.InitialDebt));
            }
        }

        
        public int HrubyVynos => ((int)((realEstate.MonthlyRent * 12) / porizovaciCena));

        
        public double RocniNavratnost => (porizovaciCena / cistyRocniVynos);


        public double CistyVynos { get
            {
                if(cistyRocniVynos == 0) return 0;
                return (porizovaciCena / cistyRocniVynos);
            } 
        } 


        /*
        public double Rentabilita()
        {
            return (cistyRocniVynos / porizovaciCena);
        }*/




    }
}
