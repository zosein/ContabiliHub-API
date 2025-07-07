using ContabiliHub.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContabiliHub.API.Filters
{
    public class ValidateModelAttribute<T> : ActionFilterAttribute where T : class
    {
        private readonly IValidator<T> _validator;

        public ValidateModelAttribute(IValidator<T> validator)
        {
            _validator = validator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var model = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

            if (model == null)
            {
                context.Result = new BadRequestObjectResult(new { message = "Modelo inválido ou ausente." });
                return;
            }

            var validationResult = _validator.Validate(model);

            if (!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "Modelo inválido.",
                    errors = validationResult.Errors
                });
            }
        }
    }
}