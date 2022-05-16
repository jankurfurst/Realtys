using FluentValidation;
using Realtys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtys.Validations
{
    public class MortgageValidations : AbstractValidator<Mortgage>
    {
        public MortgageValidations()
        {
            //Pravidlo pro Roční úroková sazba
            RuleFor(x => x.Interest)
                .NotEmpty().WithMessage("Roční úroková sazba je povinný údaj.")
                .InclusiveBetween(1, 100).WithMessage("Roční úroková sazba musí být v rozmezí 1-100.");

            //Pravidlo pro Podíl z ceny nemovitosti
            RuleFor(x => x.Share)
                .NotEmpty().WithMessage("Podíl z ceny nemovitosti je povinný údaj.")
                .InclusiveBetween(1,100).WithMessage("Podíl z ceny nemovitosti musí být v rozmezí 1-100.");

            //Pravidlo pro Počet let splácení úvěru
            RuleFor(x => x.ForYears)
                .NotEmpty().WithMessage("Počet let splácení úvěru je povinný údaj.")
                .GreaterThan(0).WithMessage("Počet let splácení úvěru musí být > 0.");

        }
    }
}
