using DecryptV3.Dtos;
using FluentValidation;
using System.Collections.Generic;

public class DecryptDataRequestDtoValidator : AbstractValidator<DecryptDataRequestDto>
{
    public DecryptDataRequestDtoValidator()
    {

        //required fields
        RuleFor(x => x.CustomerCode).NotNull().NotEmpty();
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.EncryptedData).NotNull().NotEmpty();
        RuleFor(x => x.KSN).NotNull().NotEmpty();
        RuleFor(x => x.KeyType).Must(x =>
        {
            var keyTypes = new List<string> { "Pin", "Data" };
            return keyTypes.Contains(x);
        }).WithMessage("'KeyType' Invalid");

    }

}