using FluentValidation;
using Realtys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtys.Validations
{
    internal class EditRealtyValidations : AbstractValidator<RealEstate>
    {

        public EditRealtyValidations()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Název nemovitosti je požadován.");

            RuleFor(x => x.RealtyPrice)
                .NotNull().WithMessage("Cena nemovitosti je požadována.")
                .GreaterThanOrEqualTo(0).WithMessage("Cena nemovitosti musí být vyšší nebo rovna 0.");

            RuleFor(x => x.Vacancy)
                .NotNull().WithMessage("Hodnota neobsazenosti je požadováno (0-12).")
                .InclusiveBetween(0, 12).WithMessage("Neobsazenost nemovitosti musí být mezi 0 a 12 měsíci.");

        }

    }
}
