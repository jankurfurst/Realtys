using Realtys.Models;
using Realtys.Views;
using System.ComponentModel;

namespace Realtys.ViewModels
{
    public class DetailViewModel : INotifyPropertyChanged
    {
        #region Fields
        private Command _DeleteCommand;
        private Command _EditCommand;
        private Command _ResetCommand;
        private int cenaNemovitosti;
        private int mesNajem;
        private int mesNaklady;
        private double pocatecniDluh;
        private double rocniUrokovaSazba;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public RealEstate RealEstate { get; set; }
        public Mortgage Mortgage { get; set; }

        // Properties pro zobrazení údajů nemovitosti
        public int CenaNemovitosti
        {
            get => cenaNemovitosti; set
            {
                cenaNemovitosti = value;
                OnPropertyChanged(nameof(CenaNemovitosti));
                OnPropertyChanged(nameof(PorizovaciCena));
                OnPropertyChanged(nameof(RocniRustVlastnihoJmeni));
                OnPropertyChanged(nameof(HrubyVynos));
                OnPropertyChanged(nameof(RocniNavratnostVlastnichZdroju));
                OnPropertyChanged(nameof(RocniZhodnoceniVlastnichZdroju));
                OnPropertyChanged(nameof(RocniNavratnost));
                if (Mortgage != null)
                {
                    PocatecniDluhSliderEdited = value;
                    SplatkaUveru = CountPayment(RocniUrokovaSazba);
                    CountMiddleValues();

                    OnPropertyChanged(nameof(SplatkaUveru));
                    OnPropertyChanged(nameof(PocatecniDluhSliderEdited));
                    OnPropertyChanged(nameof(PocatecniDluh));
                    OnPropertyChanged(nameof(StredniHodnotaSplatkyJistiny));
                    OnPropertyChanged(nameof(StredniHodnotaSplatkyUroku));
                }

            }
        }
        public int MesNajem
        {
            get => mesNajem;
            set
            {
                mesNajem = value;
                OnPropertyChanged(nameof(MesNajem));
                OnPropertyChanged(nameof(RocniRustVlastnihoJmeni));
                OnPropertyChanged(nameof(HrubyVynos));
                OnPropertyChanged(nameof(RocniNavratnostVlastnichZdroju));
                OnPropertyChanged(nameof(RocniZhodnoceniVlastnichZdroju));
                OnPropertyChanged(nameof(RocniNavratnost));
            }
        }
        public int MesNaklady
        {
            get => mesNaklady;
            set
            {
                mesNaklady = value;
                OnPropertyChanged(nameof(MesNaklady));
                OnPropertyChanged(nameof(RocniRustVlastnihoJmeni));
                OnPropertyChanged(nameof(HrubyVynos));
                OnPropertyChanged(nameof(RocniNavratnostVlastnichZdroju));
                OnPropertyChanged(nameof(RocniZhodnoceniVlastnichZdroju));
                OnPropertyChanged(nameof(RocniNavratnost));
            }
        }
        public double Neobsazenost { get; set; }
        public int PocetLetDrzeni { get; set; }

        //Properties pro zobrazení údajů úvěru
        public double RocniUrokovaSazba
        {
            get => rocniUrokovaSazba;

            set
            {
                rocniUrokovaSazba = value; OnPropertyChanged(nameof(RocniUrokovaSazba)); OnPropertyChanged(nameof(RocniUrokovaSazbaSliderEdited
            ));
            }
        }
        public double RocniUrokovaSazbaSliderEdited
        {
            get => rocniUrokovaSazba;
            set
            {
                rocniUrokovaSazba = value;
                PocatecniDluhSliderEdited = cenaNemovitosti;
                SplatkaUveru = CountPayment(value);
                CountMiddleValues();
                OnPropertyChanged(nameof(RocniUrokovaSazba));
                OnPropertyChanged(nameof(StredniHodnotaSplatkyJistiny));
                OnPropertyChanged(nameof(StredniHodnotaSplatkyUroku));
                OnPropertyChanged(nameof(SplatkaUveru));
                OnPropertyChanged(nameof(RocniRustVlastnihoJmeni));
                OnPropertyChanged(nameof(RocniNavratnostVlastnichZdroju));
                OnPropertyChanged(nameof(RocniZhodnoceniVlastnichZdroju));
            }
        }
        public double PodilZCeny { get; set; }
        public int PocetLetSplaceni { get; set; }
        public double PocatecniDluh { get => pocatecniDluh; set => pocatecniDluh = value; }
        public double PocatecniDluhSliderEdited
        {
            get => pocatecniDluh;
            set
            {
                if (Mortgage == null) pocatecniDluh = 0;
                else
                {
                    value *= PodilZCeny / 100;
                    pocatecniDluh = value;
                    OnPropertyChanged(nameof(PocatecniDluh));
                }
            }
        }
        public double SplatkaUveru { get; set; }



        public Command DeleteCommand => _DeleteCommand ??= new Command(ExecuteDeleteCommand);

        public Command EditCommand => _EditCommand ??= new Command(ExecuteEditCommand);

        public Command ResetCommand => _ResetCommand ??= new Command(ExecuteResetCommand);


        /// <summary>
        /// Roční růst vlastního jmění v Kč za rok
        /// </summary>
        public double RocniRustVlastnihoJmeni
        {
            get
            {
                return (double)((MesNajem - StredniHodnotaSplatkyUroku) * 12);
            }
        }

        /// <summary>
        /// Pořizovací cena nemovitosti
        /// </summary>
        public double PorizovaciCena
        {
            get
            {
                if (Mortgage == null) return CenaNemovitosti;
                return (CenaNemovitosti - PocatecniDluh);
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
                return ((MesNajem * 12.00) / (CenaNemovitosti) * 100);
            }
        }

        /// <summary>
        /// Investované vlastní zdroje
        /// </summary>
        private double VlastniZdroje
        {
            get
            {
                double neobsazenost = (MesNajem * 12.00 * Neobsazenost / 100.00);
                double prvniRokMesNakladu =
                    (MesNaklady * 12.00
                    + neobsazenost
                    + StredniHodnotaSplatkyUroku * 12);
                return prvniRokMesNakladu + PorizovaciCena;
            }
        }

        /// <summary>
        /// Roční návratnost investice
        /// </summary>
        public double RocniNavratnost
        {
            get
            {
                if (MesNajem == 0) return double.PositiveInfinity;
                return ((CenaNemovitosti / (MesNajem * 12.0)));
            }
        }

        /// <summary>
        /// Roční návratnost vlastních zdrojů v letech.
        /// </summary>
        public double RocniNavratnostVlastnichZdroju
        {
            get
            {
                return ((double)(VlastniZdroje / ((double)(MesNajem - StredniHodnotaSplatkyUroku) * 12)));
            }
        }

        /// <summary>
        /// Roční zhodnocení vlastních zdrojů v %.
        /// </summary>
        public double RocniZhodnoceniVlastnichZdroju
        {
            get
            {
                return Math.Pow(RocniNavratnostVlastnichZdroju, -1) * 100;
            }
        }


        #endregion

        #region Constructor
        public DetailViewModel(int id)
        {
            RealEstate = App.DbContext.RealEstates.FirstOrDefault(r => r.ID == id);
            Mortgage = App.DbContext.Mortgages.FirstOrDefault(m => m.RealtyID == id);

            CenaNemovitosti = Int32.Parse(RealEstate.RealtyPrice);
            MesNajem = Int32.Parse(RealEstate.MonthlyRent);
            MesNaklady = Int32.Parse(RealEstate.MonthlyExpenses);
            Neobsazenost = Double.Parse(RealEstate.Vacancy);
            PocetLetDrzeni = Int32.Parse(RealEstate.ForYears);

            if (Mortgage != null)
            {
                RocniUrokovaSazba = Double.Parse(Mortgage.Interest);
                PodilZCeny = Double.Parse(Mortgage.Share);
                PocetLetSplaceni = Int32.Parse(Mortgage.ForYears);
                PocatecniDluh = Mortgage.InitialDebt;
                SplatkaUveru = Mortgage.Payment;
                CountMiddleValues();
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Výpočet splátky
        /// </summary>
        private double CountPayment(double urok_sazba)
        {
            int pocetMesicu = PocetLetSplaceni * 12;
            double urokova_mira = (urok_sazba / 100) / 12;

            double v = 1 / (1 + urokova_mira);
            return ((urokova_mira * PocatecniDluh) / (1 - Math.Pow(v, pocetMesicu)));
        }

        /// <summary>
        /// Výpočet středních hodnot splátek úroku a jistiny
        /// </summary>
        private void CountMiddleValues()
        {
            double aktualniDluh = PocatecniDluh;
            double mes_urok_sazba = RocniUrokovaSazba / (12 * 100);
            for (int i = 0; i <= (PocetLetSplaceni * 12); i++)
            {
                double urok = mes_urok_sazba * aktualniDluh;
                double umor = SplatkaUveru - urok;

                if (i == PocetLetDrzeni / 2 * 12)
                {
                    StredniHodnotaSplatkyUroku = urok;
                    StredniHodnotaSplatkyJistiny = umor;
                    break;
                }
                aktualniDluh -= umor;
            }
        }

        /// <summary>
        /// Metoda pro použití OnPropertyChanged
        /// </summary>
        /// <param name="name">Název property</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void ExecuteResetCommand()
        {
            MesNajem = Int32.Parse(RealEstate.MonthlyRent);
            MesNaklady = Int32.Parse(RealEstate.MonthlyExpenses);
            CenaNemovitosti = Int32.Parse(RealEstate.RealtyPrice);
            if(Mortgage != null)
            {
                RocniUrokovaSazba = Double.Parse(Mortgage.Interest);
            }
        }

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
