using Realtys.Database;
using Realtys.Models;
using Realtys.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private RealtysDbContext DbContext;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public string EditCreateErrors = string.Empty;

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

        public Command SaveCommand => _SaveCommand ??= new Command(ExecuteSaveCommand);
        #endregion

        #region Constructor
        [Obsolete]
        public EditCreateViewModel(RealtysDbContext dbContext)
        {
            realtyValidation = new RealtyValidations();
            DbContext = dbContext;
        }
        #endregion

        #region Methods
        public void ExecuteSaveCommand()
        {
            EditCreateErrors = string.Empty;

            var result = realtyValidation.Validate(this.RealEstate);
            foreach (var errors in result.Errors)
            {
                EditCreateErrors = EditCreateErrors + errors + "\n";
            }

            if (result.IsValid)
            {
                EditCreateErrors = string.Empty;
                //Save Realty

            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
