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
        /// Roční růst vlastního jmění v Kč za rok
        /// </summary>
        public double rocniRustVlastnihoJmeni => (double)((Int32.Parse(realEstate.MonthlyRent) - StredniHodnotaSplatkyUroku) * 12);
        
        /// <summary>
        /// Pořizovací cena nemovitosti
        /// </summary>
        public double porizovaciCena
        {
            get
            {
                if (mortgage == null) return Int32.Parse(realEstate.RealtyPrice);
                return (Int32.Parse(realEstate.RealtyPrice) - mortgage.InitialDebt);
            }
        }

        /// <summary>
        /// Hodnota splátky úroku v polovině vlastnictví nemovitosti
        /// </summary>
        public double StredniHodnotaSplatkyUroku { get; set; }

        /// <summary>
        /// Hodnota splátky jistiny (úmor) v polovině vlastnictví nemovitosti
        /// </summary>
        public double StredniHodnotaSplatkyJistiny { get; set; }


        /// <summary>
        /// Hrubý výnos v % za rok
        /// </summary>
        public double HrubyVynos => ((Int32.Parse(realEstate.MonthlyRent) * 12.00) / (Int32.Parse(realEstate.RealtyPrice)) *100);

        private double VlastniZdroje
        {
            get
            {
                double neobsazenost = (Int32.Parse(realEstate.MonthlyRent) * 12.00 * Double.Parse(realEstate.Vacancy) / 100.00);
                double prvniRokMesNakladu =
                    (Int32.Parse(realEstate.MonthlyExpenses) * 12.00
                    + neobsazenost
                    + StredniHodnotaSplatkyUroku * 12);
                return prvniRokMesNakladu + porizovaciCena;
            }
        }

        /// <summary>
        /// Roční návratnost investice
        /// </summary>
        public double RocniNavratnost => ((Int32.Parse(realEstate.RealtyPrice) / (Int32.Parse(realEstate.MonthlyRent) * 12)));

        /// <summary>
        /// Roční návratnost vlastních zdrojů v letech.
        /// </summary>
        public double RocniNavratnostVlastnichZdroju
        {
            get
            {
                return((double)(VlastniZdroje / ((double)(Int32.Parse(realEstate.MonthlyRent) - StredniHodnotaSplatkyUroku) * 12)));
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


        #endregion

        #region Constructor
        public DetailViewModel(int id)
        {
            realEstate = App.DbContext.RealEstates.FirstOrDefault(r => r.ID == id); 
            mortgage = App.DbContext.Mortgages.FirstOrDefault(m => m.RealtyID == id);

            if (mortgage != null)
            {
                double aktualniDluh = (double)mortgage.InitialDebt;
                double mes_urok_sazba = Double.Parse(mortgage.Interest) / (12 * 100);
                for (int i = 0; i <= (Int32.Parse(mortgage.ForYears) * 12); i++)
                {
                    double urok = mes_urok_sazba * aktualniDluh;
                    double umor = Mes_splatka_hypo - urok;
                    
                    if (i == Int32.Parse(realEstate.ForYears) / 2 * 12)
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
                RealEstate = new()
                {
                    ID = realEstate.ID,
                    Name = realEstate.Name,
                    MonthlyExpenses = realEstate.MonthlyExpenses,
                    MonthlyRent = realEstate.MonthlyRent,
                    RealtyPrice = realEstate.RealtyPrice,
                    Vacancy = realEstate.Vacancy,
                    ForYears = realEstate.ForYears,
                    MortgageUsage = realEstate.MortgageUsage
                },                
                Mortgage = mortgage == null 
                ? new Mortgage()
                : new() 
                { 
                    ID = mortgage.ID,
                    Interest = mortgage.Interest,
                    InitialDebt = mortgage.InitialDebt,
                    Payment = mortgage.Payment,
                    Share = mortgage.Share,
                    ForYears = mortgage.ForYears,
                    RealtyID = mortgage.RealtyID
                },
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
