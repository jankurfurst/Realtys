﻿using FluentValidation;
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
               .GreaterThanOrEqualTo(0).WithMessage("Měsíční náklady musí být vyšší nebo rovny 0.");

            RuleFor(x => x.MonthlyRent)
                .NotNull().WithMessage("Měsíční nájem je požadován.")
                .GreaterThanOrEqualTo(0).WithMessage("Měsíční nájem musí být vyšší nebo roven 0.");

            RuleFor(x => x.RealtyPrice)
                .NotNull().WithMessage("Cena nemovitosti je požadována.")
                .GreaterThanOrEqualTo(0).WithMessage("Cena nemovitosti musí být vyšší nebo rovna 0.");

            RuleFor(x => x.Vacancy)
                .NotNull().WithMessage("Hodnota neobsazenosti je požadováno (0-12).")
                .InclusiveBetween(0, 12).WithMessage("Neobsazenost nemovitosti musí být mezi 0 a 12 měsíci.");

        }

    }
}
