using Carola.EntityLayer.Entites;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.ValidationRules
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator() 
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adı boş geçilemez!");
            RuleFor(x => x.CategoryName).MinimumLength(2).WithMessage("Lütfen en az iki karakter girişi yapınız").MaximumLength(20).WithMessage("Lütfen en fazla 20 karakter girişi yapınız.");
        }

    }
}
