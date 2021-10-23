using FluentValidation;

namespace Blog.Application.Queries.RefreshToken
{
    public class RefreshTokenQueryValidator : AbstractValidator<RefreshTokenQuery>
    {
        public RefreshTokenQueryValidator()
        {
            RuleFor(x => x.Token)
                .NotNull()
                .NotEmpty();
        }
    }
}
