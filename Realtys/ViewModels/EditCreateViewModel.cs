using Realtys.Database;
using Realtys.Models;
using Realtys.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtys.ViewModels
{
    /// <summary>
    /// ViewModel pro vytváření a editaci záznamů
    /// </summary>
    public class EditCreateViewModel : INotifyPropertyChanged
    {
        #region Fields
        private RealEstate _RealEstate;
        private Mortgage _Mortgage;
        CreateRealtyValidations createRealtyValidation;
        EditRealtyValidations editRealtyValidation;
        MortgageValidations mortgageValidation;
        private Command _SaveCommand;
        private string _errors;
        private bool _mortgageUsage;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly RealtysDbContext DbContext;

        /// <summary>
        /// Property pro error zprávy.
        /// </summary>
        public string EditCreateErrors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                OnPropertyChanged(nameof(EditCreateErrors));
            }
        }

        public RealEstate RealEstate
        {
            get { return _RealEstate; }
            set
            {
                _RealEstate = value;
                OnPropertyChanged(nameof(RealEstate));
            }
        }

        public Mortgage Mortgage
        {
            get { return _Mortgage; }
            set
            {
                _Mortgage = value;
                OnPropertyChanged(nameof(Mortgage));
            }
        }

        /// <summary>
        /// Property pro binding na checkbox ve View.
        /// </summary>
        public bool IsMortgageUsed
        {
            get { return _mortgageUsage; }
            set
            {
                _mortgageUsage = value;
                OnPropertyChanged(name: nameof(IsMortgageUsed));
            }
        }

        /// <summary>
        /// Command pro uložení do databáze
        /// </summary>
        public Command SaveCommand => _SaveCommand ??= new Command(ExecuteSaveCommand);
        #endregion

        #region Constructor
        /// <summary>
        /// Konstruktor EditCreateViewModel.
        /// </summary>
        public EditCreateViewModel()
        {
            createRealtyValidation = new CreateRealtyValidations();
            editRealtyValidation = new EditRealtyValidations();
            mortgageValidation = new MortgageValidations();

            this.RealEstate = new RealEstate();
            this.Mortgage = new Mortgage();
            DbContext = App.DbContext;
        }
        #endregion

        #region Methods
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

        /// <summary>
        /// Metoda pro uložení záznamu do databáze.
        /// </summary>
        public async void ExecuteSaveCommand()
        {
            EditCreateErrors = string.Empty;

            //Zobrazení DisplayAlert-u pro potvrzení uložení záznamu
            var status = await Shell.Current.DisplayAlert("Uložení záznamu", $"Přejete si aby byl záznam {this.RealEstate.Name} uložen?", "Uložit", "Zrušit");
            if (!status) return;

            //Validace vstupů pro nemovitost (pro vytvoření nebo editaci)
            var result = (RealEstate.ID == 0)
                ? createRealtyValidation.Validate(this.RealEstate)
                : editRealtyValidation.Validate(this.RealEstate);
            
            //Ověření validních vstupů
            if (result.IsValid)
            {
                EditCreateErrors = string.Empty;

                var editRealty = DbContext.RealEstates.FirstOrDefault(r => r.ID == this.RealEstate.ID);
                //Editace záznamu
                if (editRealty != null)
                {
                    editRealty.Name = this.RealEstate.Name;
                    editRealty.RealtyPrice = this.RealEstate.RealtyPrice;
                    editRealty.MonthlyExpenses = this.RealEstate.MonthlyExpenses;
                    editRealty.MonthlyRent = this.RealEstate.MonthlyRent;
                    editRealty.Vacancy = this.RealEstate.Vacancy;
                    editRealty.MortgageUsage = this.IsMortgageUsed;

                    await DbContext.SaveChangesAsync();
                }
                //Vytvoření nového záznamu
                else
                {
                    this.RealEstate.MortgageUsage = this.IsMortgageUsed;
                    DbContext.RealEstates.Add(this.RealEstate);
                    await DbContext.SaveChangesAsync();
                }

            }
            //Přiřazení error zpráv, pokud vznikly errory při validaci vstupů pro nemovitost
            else
            {
                foreach (var error in result.Errors)
                {
                    EditCreateErrors = EditCreateErrors + error + "\n";
                }
            }

            //Save Mortgage to DB if used
            if (IsMortgageUsed)
            {
                var mortResult = mortgageValidation.Validate(Mortgage);

                //Ověření validních vstupů
                if (mortResult.IsValid && result.IsValid)
                {
                    var mortgage = DbContext.Mortgages.FirstOrDefault(_m => _m.RealtyID == this.RealEstate.ID);

                    double urok = (double)this.Mortgage.Interest;// %
                    double podil = (double)this.Mortgage.Share;// %
                    double pocatecniDluh = (double)(this.RealEstate.RealtyPrice * (podil / 100.0));
                    int pocetLet = (int)this.Mortgage.ForYears;

                    int pocetMesicu = pocetLet * 12;
                    double urokova_mira = (urok / 100) / 12;

                    double v = 1 / (1 + urokova_mira);
                    double splatka = ((urokova_mira * pocatecniDluh) / (1 - Math.Pow(v, pocetMesicu)));

                    //Editace již existujícího záznamu
                    if (mortgage != null)
                    {
                        mortgage.Interest = urok;
                        mortgage.Share = podil;
                        mortgage.InitialDebt = pocatecniDluh;
                        mortgage.ForYears = pocetLet;
                        mortgage.Payment = splatka;

                        await DbContext.SaveChangesAsync();
                    }
                    //Vytvoření nového záznamu
                    else
                    {                        
                        this.Mortgage.InitialDebt = pocatecniDluh;
                        this.Mortgage.Payment = splatka;
                        this.Mortgage.RealtyID = this.RealEstate.ID;
                        DbContext.Mortgages.Add(this.Mortgage);
                        await DbContext.SaveChangesAsync();
                    }

                }
                //Přiřazení error zpráv, pokud vznikly errory při validaci vstupů pro úvěr
                else
                {
                    foreach (var error in mortResult.Errors)
                    {
                        EditCreateErrors = EditCreateErrors + error + "\n";
                    }
                }

            }
            // Vymazání úvěru k nemovitosti, pokud byl při editaci odstraněn
            else
            {
                var mortgage = DbContext.Mortgages.FirstOrDefault(_m => _m.RealtyID == this.RealEstate.ID);
                // Pokud existuje v databázi úvěr, který není potřeba, odstraní se
                if (mortgage != null)
                {
                    DbContext.Mortgages.Remove(mortgage);
                    await DbContext.SaveChangesAsync();
                }
            }

            // Zobrazení zpráv DisplayAllert-u, pokud vznikly errory při validaci vstupů
            if (EditCreateErrors != String.Empty)
            {
                await Shell.Current.DisplayAlert("Chyba při validaci", this.EditCreateErrors, "Zpět");
            }
            //reseting properties and return to List
            else
            {

                this.RealEstate = new RealEstate();
                this.Mortgage = new Mortgage();
                this.IsMortgageUsed = false;

                await Shell.Current.GoToAsync("..");
                await Shell.Current.GoToAsync("//first");
            }

        }

        #endregion
    }
}
