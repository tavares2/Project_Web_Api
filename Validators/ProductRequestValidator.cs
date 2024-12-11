using FluentValidation;
using LGC_CodeChallenge.Contracts;

namespace LGC_CodeChallenge.Validators
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");

        }    
    }
}
