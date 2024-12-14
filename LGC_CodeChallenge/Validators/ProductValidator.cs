using FluentValidation;
using LGC_CodeChallenge.Models;
namespace LGC_CodeChallenge.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Stock).GreaterThan(0).WithMessage("Stock must be greater than 0");
        }
    }
}
