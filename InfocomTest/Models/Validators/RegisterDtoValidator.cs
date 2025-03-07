using FluentValidation;
using InfocomTest.Models.DTOs;

namespace InfocomTest.Models.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.UserName)
           .NotEmpty().WithMessage("Имя пользователя обязательно для заполнения.")
           .MinimumLength(3).WithMessage("Имя пользователя должно содержать минимум 3 символа.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
            .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов.")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
            .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
            .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру.")
            .Matches("[@#$%^&*!]").WithMessage("Пароль должен содержать хотя бы один специальный символ (@#$%^&*!).");
    }
}