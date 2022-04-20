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
    public class EditCreateViewModel : INotifyPropertyChanged
    {
        #region Fields
        private RealEstate _RealEstate;
        private Mortgage _Mortgage;
        RealtyValidations realtyValidation;
        private Command _SaveCommand;
        private string _errors;
        private bool _mortgageUsage;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly RealtysDbContext DbContext;

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

        public bool IsMortgageUsed
        {
            get { return _mortgageUsage; }
            set
            {
                _mortgageUsage = value;
                OnPropertyChanged(name: nameof(IsMortgageUsed));
            }
        }

        public Command SaveCommand => _SaveCommand ??= new Command(ExecuteSaveCommand);
        #endregion

        #region Constructor
        [Obsolete]
        public EditCreateViewModel()
        {
            realtyValidation = new RealtyValidations();
            this.RealEstate = new RealEstate();
            this.Mortgage = new Mortgage();
            DbContext = App.DbContext;
        }
        #endregion

        #region Methods
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        public async void ExecuteSaveCommand()
        {
            EditCreateErrors = string.Empty;

            var status = await Shell.Current.DisplayAlert("Uložení záznamu", $"Přejete si aby byl záznam {this.RealEstate.Name} uložen?", "Uložit", "Zrušit");
            if (!status) return;

            var result = realtyValidation.Validate(this.RealEstate);

            if (result.IsValid)
            {
                EditCreateErrors = string.Empty;

                var editRealty = DbContext.RealEstates.FirstOrDefault(r => r.ID == this.RealEstate.ID);
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
                else
                {
                    //Save Realty to DB
                    this.RealEstate.MortgageUsage = this.IsMortgageUsed;
                    DbContext.RealEstates.Add(this.RealEstate);
                    await DbContext.SaveChangesAsync();
                }


                //Save Mortgage to DB if used
                if (IsMortgageUsed)
                {
                    var mortgage = DbContext.Mortgages.FirstOrDefault(_m => _m.RealtyID == this.RealEstate.ID);
                    if (mortgage != null)
                    {
                        mortgage.Share = this.Mortgage.Share;
                        mortgage.MonthlyInterest = this.Mortgage.MonthlyInterest;
                        mortgage.Share = this.Mortgage.Share;
                        mortgage.InitialDebt = this.Mortgage.InitialDebt;
                        mortgage.ForYears = this.Mortgage.ForYears;
                        mortgage.Payment = this.Mortgage.Payment;

                        await DbContext.SaveChangesAsync();
                    }
                    else
                    {
                        this.Mortgage.RealtyID = this.RealEstate.ID;
                        DbContext.Mortgages.Add(this.Mortgage);
                        await DbContext.SaveChangesAsync();
                    }

                }

                //reseting properties and return to List
                this.RealEstate = new RealEstate();
                this.Mortgage = new Mortgage();
                this.IsMortgageUsed = false;

                await Shell.Current.GoToAsync("..");
                //await Shell.Current.Navigation.PopAsync();
                //await Shell.Current.GoToAsync("//first");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    EditCreateErrors = EditCreateErrors + error + "\n";
                }
                await Shell.Current.DisplayAlert("Chyba při validaci", this.EditCreateErrors, "Zpět");
            }
        }

        #endregion
    }
}
