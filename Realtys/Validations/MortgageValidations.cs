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
            RuleFor(x => x.Interest).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Roční úroková sazba je povinný údaj.");
                }
                else
                {
                    if (!Double.TryParse(value, out double i) || i < 1 || i > 100) 
                        context.AddFailure("Roční úroková sazba musí být v rozmezí 1-100.");
                }
            });

            //Pravidlo pro Podíl z ceny nemovitosti
            RuleFor(x => x.Share).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Podíl z ceny nemovitosti je povinný údaj.");
                }
                else
                {
                    if (!Double.TryParse(value, out double i) || i < 1 || i > 100)
                        context.AddFailure("Podíl z ceny nemovitosti musí být v rozmezí 1-100.");
                }
            });

            //Pravidlo pro Počet let splácení úvěru
            RuleFor(x => x.ForYears).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Počet let splácení úvěru je povinný údaj.");
                }
                else
                {
                    if (!Int32.TryParse(value, out int i) || i < 0)
                        context.AddFailure("Počet let splácení úvěru musí být > 0.");
                }
            });

        }
    }
}
