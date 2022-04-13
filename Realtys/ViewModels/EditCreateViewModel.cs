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
            this.Mortgage= new Mortgage();
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


        public void ExecuteSaveCommand()
        {
            EditCreateErrors = string.Empty;

            var result = realtyValidation.Validate(this.RealEstate);
            foreach (var error in result.Errors)
            {
                EditCreateErrors = EditCreateErrors + error + "\n";
            }
            Debug.WriteLine(EditCreateErrors);
            if (result.IsValid)
            {
                EditCreateErrors = string.Empty;
                //Save Realty

            }
        }

        #endregion
    }
}
