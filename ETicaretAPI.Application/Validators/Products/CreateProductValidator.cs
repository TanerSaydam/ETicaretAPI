using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage("Ürünün adı boş olamaz!")
                .NotNull()
                    .WithMessage("Ürünün adı boş olamaz!")
                .MaximumLength(150)
                .MinimumLength(2)
                    .WithMessage("Ürün adı en az 2 karakter olmalıdır!");

            RuleFor(c => c.Stock)
               .NotEmpty()
                    .WithMessage("Stok bilgisi boş geçilemez!")
               .NotNull()
                   .WithMessage("Stok bilgisi boş geçilemez!")
               .Must(s=> s>=0)
                   .WithMessage("Stok bilgisi en az 0 olmalıdır!");

            RuleFor(c => c.Price)
               .NotEmpty()
                    .WithMessage("Fiyat bilgisi boş geçilemez!")
               .NotNull()
                   .WithMessage("Fiyat bilgisi boş geçilemez!")
               .Must(s => s >= 0)
                   .WithMessage("Fiyat bilgisi en az 0 olmalıdır!");
        }
    }
}
