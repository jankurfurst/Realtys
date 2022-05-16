using FluentValidation;
using Realtys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtys.Validations
{
    public class EditRealtyValidations : AbstractValidator<RealEstate>
    {

        public EditRealtyValidations()
        {
            //Pravidlo pro Název nemovitosti
            RuleFor(x => x.Name).NotEmpty().WithMessage("Název nemovitosti je požadován.");

            //Pravidlo pro Měsíční náklady
            RuleFor(x => x.MonthlyExpenses)
                .NotEmpty().WithMessage("Měsíční náklady jsou povinný údaj.")
                .GreaterThanOrEqualTo(0).WithMessage("Měsíční náklady musí být >= 0.");

            //Pravidlo pro Měsíční nájem
            RuleFor(x => x.MonthlyRent)
                .NotEmpty().WithMessage("Měsíční nájem je povinný údaj.")
                .GreaterThanOrEqualTo(0).WithMessage("Měsíční nájem musí být >= 0.");
                

            //Pravidlo pro Cenu nemovitosti
            RuleFor(x => x.RealtyPrice)
                .NotEmpty().WithMessage("Cena nemovitosti je povinný údaj.")
                .GreaterThanOrEqualTo(0).WithMessage("Cena nemovitosti musí být >= 0.");

            //Pravidlo pro Neobsazenost nemovitosti
            RuleFor(x => x.Vacancy)
                .NotEmpty().WithMessage("Hodnota neobsazenosti je povinný údaj.")
                .InclusiveBetween(0,100).WithMessage("Neobsazenost musí být číslo v rozmezí [0; 100].");
                
            //Pravidlo pro Počet let držení nemovitosti
            RuleFor(x => x.ForYears)
                .NotEmpty().WithMessage("Počet let, na jak dlouho bude nemovitost držena, je povinný údaj.")
                .GreaterThan(0).WithMessage("Počet let, na jak dlouho bude nemovitost držena, musí být > 0");
                
        }

    }
}
