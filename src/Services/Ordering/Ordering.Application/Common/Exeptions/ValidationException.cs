using FluentValidation.Results;

namespace Ordering.Application.Common.Exeptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have orrcured")
    {
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
    {
        Errors = failures
            .GroupBy(
                e => e.PropertyName, 
                e => e.ErrorMessage
                )
            .ToDictionary(
                failureGroup => failureGroup.Key, 
                failureGroup => failureGroup.ToArray()
                );
    }

    public IDictionary<string, string[]> Errors { get;}
}
