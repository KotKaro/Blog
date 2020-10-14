using FluentValidation;

namespace Blog.Application.Queries.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty();
        }
    }
}
