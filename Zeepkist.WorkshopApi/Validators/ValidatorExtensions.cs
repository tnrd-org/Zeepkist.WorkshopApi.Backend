﻿namespace TNRD.Zeepkist.WorkshopApi.Validators;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> IsUnsignedLong<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder
    )
    {
        return ruleBuilder.SetValidator(new IsUnsignedLongValidator<T, TProperty>());
    }
}
