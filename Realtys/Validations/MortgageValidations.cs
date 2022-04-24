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
            RuleFor(x => x.Interest)
                    .NotNull().WithMessage("Roční úroková sazba je požadována.")
                    .InclusiveBetween(0, 100).WithMessage("Roční úroková sazba musí být v rozmezí 0-100.");

            RuleFor(x => x.Share)
                    .NotNull().WithMessage("Podíl z ceny je požadován.")
                    .InclusiveBetween(0, 100).WithMessage("Podíl z ceny musí být v rozmezí 0-100.");

            RuleFor(x => x.ForYears)
                    .NotNull().WithMessage("Počet let je požadován.")
                    .GreaterThan(0).WithMessage("Počet let musí být > 0.");

        }
    }
}
