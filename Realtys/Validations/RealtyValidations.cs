using FluentValidation;
using Realtys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtys.Validations
{
    public class RealtyValidations : AbstractValidator<RealEstate>
    {
        [Obsolete]
        public RealtyValidations()
        {
           // RuleFor(x => x.Name).NotNull().WithMessage("Název nemovitosti je požadován.");
            RuleFor(x => x.RealtyPrice).NotNull().GreaterThanOrEqualTo(0).WithMessage("Cena nemovitosti musí být vyšší nebo rovna 0.");
            RuleFor(x => x.Vacancy).NotNull().InclusiveBetween(0,12).WithMessage("Neobsazenost nemovitosti musí být mezi 0 a 12 měsíci.");
            
            RuleFor(x => x.Name).Custom((name, context) => {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var realty = App.DbContext.RealEstates.FirstOrDefault(r => r.Name.ToUpper() == name.ToUpper());
                    if (realty != null)
                    {
                        context.AddFailure("Název nemovitosti je již použit.");
                    }

                }
                else
                {
                    context.AddFailure("Název nemovitosti je požadován.");
                }
            });
        }

    }
}
