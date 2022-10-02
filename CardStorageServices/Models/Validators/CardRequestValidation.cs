using CardStorageServices.Models.Request.CardRequestResponse;
using FluentValidation;

namespace CardStorageServices.Models.Validators
{
    public class CardRequestValidation: AbstractValidator<CreateCardRequest>
    {
        public CardRequestValidation()
        {
            RuleFor(x => x.CardNo)
                .NotNull()
                .CreditCard();

            RuleFor(x => x.CVV2)
                .NotNull();
            RuleFor(x => x.Name)
                .NotNull()
                .Length(3,6);
        }
    }
}
