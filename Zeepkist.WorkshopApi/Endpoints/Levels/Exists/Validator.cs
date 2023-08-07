﻿using TNRD.Zeepkist.WorkshopApi.Validators;

namespace TNRD.Zeepkist.WorkshopApi.Endpoints.Levels.Exists;

public class Validator : Validator<RequestModel>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.File).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Validation).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Gold).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Silver).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Bronze).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AuthorId).NotNull().NotEmpty().IsUnsignedLong();
        RuleFor(x => x.WorkshopId).NotNull().NotEmpty().IsUnsignedLong();
    }
}
