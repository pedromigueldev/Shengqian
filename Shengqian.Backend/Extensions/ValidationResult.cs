using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Shengqian.Backend.Extensions;
public static class ListValidationResultsExt
{
    public static ActionResult ToValidationProblem(
        this List<ValidationResult> errors)
    {
        var errorDictionary = errors
            .SelectMany(e => e.MemberNames.Select(member => new
            {
                Member = member,
                Error = e.ErrorMessage ?? "Invalid value."
            }))
            .GroupBy(e => e.Member)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Error).ToArray()
            );

        return new BadRequestObjectResult(
            new ValidationProblemDetails(errorDictionary));
    }
}