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
            RuleFor(x => x.RealtyPrice).GreaterThanOrEqualTo(0).WithMessage("Cena musí být vyšší než 0.");
            RuleFor(x => x.Vacancy).InclusiveBetween(0,12).WithMessage("Neobsazenost musí být mezi 0 a 12 měsíci.");
            
            RuleFor(x => x.Name).Custom((name, context) => {
                if (name == null) context.AddFailure("Název nemovitosti je požadován.");
                else
                {
                    var realty = App.DbContext.RealEstates.Where(r => r.Name == name);
                    if (realty != null)
                    {
                        context.AddFailure("Název nemovitosti je již použit.");
                    }

                }
            });
        }

    }
}
