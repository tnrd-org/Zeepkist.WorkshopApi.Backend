using FluentValidation.Results;

namespace TNRD.Zeepkist.WorkshopApi.Backend.Extensions;

public class ValidationResponse
{
    public string Property { get; init; } = default!;
    public string Message { get; init; } = default!;
}

public static class ValidationExtensions
{
    public static List<ValidationResponse> ToResponse(this IEnumerable<ValidationFailure> errors)
    {
        var list = new List<ValidationResponse>();

        foreach (var error in errors)
        {
            list.Add(new ValidationResponse
            {
                Property = error.PropertyName,
                Message = error.ErrorMessage
            });
        }

        return list;
    }
}
