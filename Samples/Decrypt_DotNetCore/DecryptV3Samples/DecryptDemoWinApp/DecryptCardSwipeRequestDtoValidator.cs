using DecryptV3.Dtos;
using FluentValidation;
using System.Collections.Generic;

public class DecryptCardSwipeRequestDtoValidator : AbstractValidator<DecryptCardSwipeRequestDto>
{
    public DecryptCardSwipeRequestDtoValidator()
    {

        //required fields
        RuleFor(x => x.CustomerCode).NotNull().NotEmpty();
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.DeviceSN).NotNull().NotEmpty();
        RuleFor(x => x.KSN).NotNull().NotEmpty();
        RuleFor(x => x.KeyType).Must(x =>
        {
            var keyTypes = new List<string> { "Pin", "Data" };
            return keyTypes.Contains(x);
        }).WithMessage("'KeyType' Invalid");
        RuleFor(x => x.MagnePrint).NotNull().NotEmpty();
        RuleFor(x => x.MagnePrintStatus).NotNull().NotEmpty();
        RuleFor(x => x.Track1).NotNull().NotEmpty();
        RuleFor(x => x.Track2).NotNull().NotEmpty();
    }

}