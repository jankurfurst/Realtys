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
            RuleFor(x => x.MonthlyInterest)
                    .NotNull()//.WithMessage("Měsíční náklady jsou požadovány.")
                    .GreaterThanOrEqualTo(0)//.WithMessage("Měsíční náklady musí být vyšší nebo rovny 0.")
                    ;

            RuleFor(x => x.Share)
                    .NotNull()//.WithMessage("Měsíční nájem je požadován.")
                    .GreaterThanOrEqualTo(0)//.WithMessage("Měsíční nájem musí být vyšší nebo roven 0.")
                    ;

            RuleFor(x => x.ForYears)
                    .NotNull()//.WithMessage("Měsíční nájem je požadován.")
                    .GreaterThanOrEqualTo(0)//.WithMessage("Měsíční nájem musí být vyšší nebo roven 0.")
                    ;

        }
    }
}
