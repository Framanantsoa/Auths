using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Utils;
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) {
        if(!context.ModelState.IsValid) {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    k => k.Key,
                    v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = ResponseApi.Fail(
                "Erreur de validation", errors
            );

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
