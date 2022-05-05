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
        public RealEstate RealEstate { get; set; }
        public Mortgage Mortgage { get; set; }

        public Command DeleteCommand => _DeleteCommand ??= new Command(ExecuteDeleteCommand);

        public Command EditCommand => _EditCommand ??= new Command(ExecuteEditCommand);

        private double Mes_splatka_hypo
        {
            get
            {
                if (Mortgage != null) return Mortgage.Payment;
                return 0;
            }
        }

        /// <summary>
        /// Roční růst vlastního jmění v Kč za rok
        /// </summary>
        public double RocniRustVlastnihoJmeni
        {
            get
            {
                return (double)((Int32.Parse(RealEstate.MonthlyRent) - StredniHodnotaSplatkyUroku) * 12);
            }
        }

        /// <summary>
        /// Pořizovací cena nemovitosti
        /// </summary>
        public double PorizovaciCena
        {
            get
            {
                if (Mortgage == null) return Int32.Parse(RealEstate.RealtyPrice);
                return (Int32.Parse(RealEstate.RealtyPrice) - Mortgage.InitialDebt);
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
        public double HrubyVynos
        {
            get
            {
                return ((Int32.Parse(RealEstate.MonthlyRent) * 12.00) / (Int32.Parse(RealEstate.RealtyPrice)) * 100);
            }
        }

        private double VlastniZdroje
        {
            get
            {
                double neobsazenost = (Int32.Parse(RealEstate.MonthlyRent) * 12.00 * Double.Parse(RealEstate.Vacancy) / 100.00);
                double prvniRokMesNakladu =
                    (Int32.Parse(RealEstate.MonthlyExpenses) * 12.00
                    + neobsazenost
                    + StredniHodnotaSplatkyUroku * 12);
                return prvniRokMesNakladu + PorizovaciCena;
            }
        }

        /// <summary>
        /// Roční návratnost investice
        /// </summary>
        public double RocniNavratnost {
            get
            {
                return ((Int32.Parse(RealEstate.RealtyPrice) / (Int32.Parse(RealEstate.MonthlyRent) * 12)));
            }
        }

        /// <summary>
        /// Roční návratnost vlastních zdrojů v letech.
        /// </summary>
        public double RocniNavratnostVlastnichZdroju
        {
            get
            {
                return((double)(VlastniZdroje / ((double)(Int32.Parse(RealEstate.MonthlyRent) - StredniHodnotaSplatkyUroku) * 12)));
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
            RealEstate = App.DbContext.RealEstates.FirstOrDefault(r => r.ID == id); 
            Mortgage = App.DbContext.Mortgages.FirstOrDefault(m => m.RealtyID == id);

            if (Mortgage != null)
            {
                double aktualniDluh = (double)Mortgage.InitialDebt;
                double mes_urok_sazba = Double.Parse(Mortgage.Interest) / (12 * 100);
                for (int i = 0; i <= (Int32.Parse(Mortgage.ForYears) * 12); i++)
                {
                    double urok = mes_urok_sazba * aktualniDluh;
                    double umor = Mes_splatka_hypo - urok;
                    
                    if (i == Int32.Parse(RealEstate.ForYears) / 2 * 12)
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
            var status = await Shell.Current.DisplayAlert("Editace záznamu", $"Přejete si upravit záznam {this.RealEstate.Name}?", "Upravit", "Zrušit");
            if (!status) return;

            var viewModel = new EditCreateViewModel()
            {
                RealEstate = new()
                {
                    ID = RealEstate.ID,
                    Name = RealEstate.Name,
                    MonthlyExpenses = RealEstate.MonthlyExpenses,
                    MonthlyRent = RealEstate.MonthlyRent,
                    RealtyPrice = RealEstate.RealtyPrice,
                    Vacancy = RealEstate.Vacancy,
                    ForYears = RealEstate.ForYears,
                    MortgageUsage = RealEstate.MortgageUsage
                },                
                Mortgage = Mortgage == null 
                ? new Mortgage()
                : new() 
                { 
                    ID = Mortgage.ID,
                    Interest = Mortgage.Interest,
                    InitialDebt = Mortgage.InitialDebt,
                    Payment = Mortgage.Payment,
                    Share = Mortgage.Share,
                    ForYears = Mortgage.ForYears,
                    RealtyID = Mortgage.RealtyID
                },
                IsMortgageUsed = RealEstate.MortgageUsage
            };
            await Shell.Current.Navigation.PushAsync(new EntryPage(viewModel));


        }

        private async void ExecuteDeleteCommand()
        {
            var status = await Shell.Current.DisplayAlert("Odstranění záznamu", $"Přejete si aby byl záznam {this.RealEstate.Name} odstraněn?", "Odstranit", "Zrušit");
            if (!status) return;

            var r = this.RealEstate;
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
