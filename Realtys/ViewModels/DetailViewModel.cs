using Realtys.Models;
using Realtys.Views;

namespace Realtys.ViewModels
{
    public class DetailViewModel
    {
        #region Fields
        private Command _DeleteCommand;
        private Command _EditCommand;
        #endregion

        #region Properties
        public RealEstate realEstate { get; set; }
        public Mortgage mortgage { get; set; }

        public Command DeleteCommand => _DeleteCommand ??= new Command(ExecuteDeleteCommand);

        public Command EditCommand => _EditCommand ??= new Command(ExecuteEditCommand);

        private double Mes_splatka_hypo
        {
            get
            {
                if (mortgage != null) return mortgage.Payment;
                return 0;
            }
        }

        /// <summary>
        /// Čistý výnos v Kč za rok
        /// </summary>
        public double cistyRocniVynos => (double)((realEstate.MonthlyRent - Mes_splatka_hypo - realEstate.MonthlyExpenses) * (12 * (1 - (double)realEstate.Vacancy/100)));
        
        /// <summary>
        /// Pořizovací cena nemovitosti
        /// </summary>
        public int porizovaciCena
        {
            get
            {
                if (mortgage == null) return (int)realEstate.RealtyPrice;
                return (int)(realEstate.RealtyPrice - Math.Floor(mortgage.InitialDebt));
            }
        }

        /// <summary>
        /// Hodnota splátky úroku v polovině vlastnictví nemovitosti
        /// </summary>
        private double StredniHodnotaSplatkyUroku { get; set; }

        /// <summary>
        /// Hodnota splátky jistiny (úmor) v polovině vlastnictví nemovitosti
        /// </summary>
        private double StredniHodnotaSplatkyJistiny { get; set; }


        /// <summary>
        /// Hrubý výnos v % za rok
        /// </summary>
        public double HrubyVynos => ((double)((realEstate.MonthlyRent * 12.00) / porizovaciCena))*100;

        private double VlastniZdroje
        {
            get
            {
                double neobsazenost = (double)(realEstate.MonthlyRent * 12.00 * realEstate.Vacancy / 100.00);
                double prvniRokMesNakladu =
                    (double)(realEstate.MonthlyExpenses * 12.00
                    + neobsazenost
                    + StredniHodnotaSplatkyUroku * 12);
                return prvniRokMesNakladu + porizovaciCena;
            }
        }

        /// <summary>
        /// Roční návratnost investice
        /// </summary>
        public double RocniNavratnost => (porizovaciCena / cistyRocniVynos);

        /// <summary>
        /// Roční návratnost vlastních zdrojů v letech.
        /// </summary>
        public double RocniNavratnostVlastnichZdroju
        {
            get
            {
                return((double)(VlastniZdroje / ((double)(realEstate.MonthlyRent - StredniHodnotaSplatkyUroku) * 12)));
            }
        }

        /// <summary>
        /// Roční zhodnocení vlastních zdrojů v %.
        /// </summary>
        public double RocniZhodnoceniVlastnichZdroju
        {
            get
            {
                return Math.Pow(RocniNavratnostVlastnichZdroju, -1)*100;
            }
        }

        /// <summary>
        /// Čistý výnos v % za rok
        /// </summary>
        public double CistyVynos { get
            {
                if (porizovaciCena == 0) return 0;
                return (cistyRocniVynos / porizovaciCena)*100;
            } 
        }

        #endregion

        #region Constructor
        public DetailViewModel(int id)
        {
            realEstate = App.DbContext.RealEstates.FirstOrDefault(r => r.ID == id);
            mortgage = App.DbContext.Mortgages.FirstOrDefault(m => m.RealtyID == id);

            if (mortgage != null)
            {
                double aktualniDluh = (double)mortgage.InitialDebt;
                double mes_urok_sazba = (double)mortgage.Interest / (12 * 100);
                for (int i = 0; i <= (mortgage.ForYears * 12); i++)
                {
                    double urok = mes_urok_sazba * aktualniDluh;
                    double umor = Mes_splatka_hypo - urok;
                    if (i == realEstate.ForYears / 2 * 12)
                    {
                        StredniHodnotaSplatkyUroku = urok;
                        StredniHodnotaSplatkyJistiny = umor;
                        break;
                    }
                    aktualniDluh -= umor;
                }
            }
            else
            {
                StredniHodnotaSplatkyUroku = 0;
                StredniHodnotaSplatkyJistiny = 0;
            }
        }
        #endregion

        #region Methods
        private async void ExecuteEditCommand()
        {
            var status = await Shell.Current.DisplayAlert("Editace záznamu", $"Přejete si upravit záznam {this.realEstate.Name}?", "Upravit", "Zrušit");
            if (!status) return;

            var viewModel = new EditCreateViewModel()
            {
                RealEstate = realEstate,
                Mortgage = mortgage == null ? new Mortgage() : mortgage,
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
        #endregion





    }
}
