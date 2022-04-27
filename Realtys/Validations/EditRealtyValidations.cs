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
            RuleFor(x => x.MonthlyExpenses).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Měsíční náklady jsou povinný údaj.");
                }
                else
                {
                    if (!Int32.TryParse(value, out int i) || i < 0) 
                        context.AddFailure("Měsíční náklady musí být >= 0.");
                }
            });


            //Pravidlo pro Měsíční nájem
            RuleFor(x => x.MonthlyRent).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Měsíční nájem je povinný údaj.");
                }
                else
                {
                    if (!Int32.TryParse(value, out int i) || i < 0) 
                        context.AddFailure("Měsíční nájem musí být >= 0.");
                }
            });

            //Pravidlo pro Cenu nemovitosti
            RuleFor(x => x.RealtyPrice).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Cena nemovitosti je povinný údaj.");
                }
                else
                {
                    if (!Int32.TryParse(value, out int i) || i < 0)
                        context.AddFailure("Cena nemovitosti musí být >= 0.");
                }
            });

            //Pravidlo pro Neobsazenost nemovitosti
            RuleFor(x => x.Vacancy).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Hodnota neobsazenosti je povinný údaj.");
                }
                else
                {
                    if (!Double.TryParse(value, out double i) || i < 0 || i > 100)
                        context.AddFailure("Neobsazenost musí být číslo v rozmezí [0; 100].");
                }
            });

            //Pravidlo pro Počet let držení nemovitosti
            RuleFor(x => x.ForYears).Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Počet let, na jak dlouho bude nemovitost držena, je povinný údaj.");
                }
                else
                {
                    if (!Int32.TryParse(value, out int i) || i <= 0)
                        context.AddFailure("Počet let, na jak dlouho bude nemovitost držena, musí být > 0");
                }
            });
        }

    }
}
