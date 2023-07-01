using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;

namespace Shared.Utilities;

public static class FluentValidationExtensions
{    
    public static async Task<IEnumerable<string>> ValidateValueAsync<T, TProp>(this IValidator<T> validator, T model, Expression<Func<T, TProp>> expression)
    {
        var memberExp = expression.Body as MemberExpression;
        var prop = memberExp?.Member as PropertyInfo;
        
        ArgumentNullException.ThrowIfNull(prop);
        
        var result = await validator.ValidateAsync(ValidationContext<T>.CreateWithOptions(model, x => x.IncludeProperties(prop!.Name)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    }
}