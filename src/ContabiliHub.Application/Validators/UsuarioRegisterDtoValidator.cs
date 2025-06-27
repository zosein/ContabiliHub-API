using ContabiliHub.Application.DTOs;
using FluentValidation;

namespace ContabiliHub.Application.Validators
{
    public class UsuarioRegisterDtoValidator : AbstractValidator<UsuarioRegisterDto>
    {
        public UsuarioRegisterDtoValidator()
        {
            RuleFor(u => u.NomeCompleto)
                .NotEmpty().WithMessage("Nome completo é obrigatório.")
                .MinimumLength(3).WithMessage("Nome completo deve ter pelo menos 3 caracteres.")
                .MaximumLength(100).WithMessage("Nome completo não pode ter mais de 100 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail deve ter um formato válido.")
                .MaximumLength(100).WithMessage("E-mail não pode ter mais de 100 caracteres.");

            RuleFor(u => u.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(50).WithMessage("Senha não pode ter mais de 50 caracteres.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
                .WithMessage("Senha deve conter pelo menos uma letra maiúscula, uma letra minúscula e um número.");
        }
    }
}