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
            RuleFor(x => x.Name).NotNull().WithMessage("Název nemovitosti je požadován.");

            RuleFor(x => x.MonthlyExpenses)
               .NotNull().WithMessage("Měsíční náklady jsou požadovány.")
               .GreaterThanOrEqualTo(0).WithMessage("Měsíční náklady musí být >= 0.");

            RuleFor(x => x.MonthlyRent)
                .NotNull().WithMessage("Měsíční nájem je požadován.")
                .GreaterThanOrEqualTo(0).WithMessage("Měsíční nájem musí být >= 0.");

            RuleFor(x => x.RealtyPrice)
                .NotNull().WithMessage("Cena nemovitosti je požadována.")
                .GreaterThanOrEqualTo(0).WithMessage("Cena nemovitosti musí být >= 0.");

            RuleFor(x => x.Vacancy)
                .NotNull().WithMessage("Hodnota neobsazenosti je požadováno (0-12).")
                .InclusiveBetween(0,100).WithMessage("Neobsazenost nemovitosti musí být v rozmezí 0-100.");

            RuleFor(x => x.ForYears)
                .NotNull().WithMessage("Počet let, na jak dlouho bude nemovitost držena, je povinný údaj.")
                .GreaterThan(0).WithMessage("Počet let, na jak dlouho bude nemovitost držena, musí být > 0");

        }

    }
}
