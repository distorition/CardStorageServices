using CardStorageServices.Models.Request.AuthenticationRequestResponse;
using FluentValidation;

namespace CardStorageServices.Models.Validators
{
  
    public class AuthenticationRequestsValidator:AbstractValidator<AuthenticationRequest> 
    {
        public AuthenticationRequestsValidator()
        {
            RuleFor(x => x.Login)
            .NotNull()
            .Length(5, 255)
            .EmailAddress();//так мы првоеряем является ли строка емайлом

            RuleFor(x => x.Password)
                .NotNull()
                .Length(5, 50);
        }
    }
}
