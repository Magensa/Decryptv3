using DecryptV3.Dtos;
using FluentValidation;
using System.Collections.Generic;

public class GenerateMacRequestDtoValidator : AbstractValidator<GenerateMacRequestDto>
{
    public GenerateMacRequestDtoValidator()
    {

        //required fields
        RuleFor(x => x.CustomerCode).NotNull().NotEmpty();
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.DataToMAC).NotNull().NotEmpty();
        RuleFor(x => x.KSN).NotNull().NotEmpty();
        RuleFor(x => x.KeyDerivationType).Must(x =>
        {
            var keyTypes = new List<string> { "DUKPT", "Fixed" };
            return keyTypes.Contains(x);
        }).WithMessage("'KeyType' Invalid");

    }

}