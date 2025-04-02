using FluentValidation;

namespace MetaExchanger.Application.Domain.Validators
{
    public class DomainOrderValidator : AbstractValidator<DomainOrder>
    {
        public DomainOrderValidator()
        {
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Time).NotEmpty();
            RuleFor(x => x.Kind).IsInEnum();
        }
    }
}