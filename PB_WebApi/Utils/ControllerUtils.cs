using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApi.Utils
{
    public static class ControllerUtils
    {
        public static bool IsValide(this ModelStateDictionary modelState, out IEnumerable<string>? errors)
        {
            if (modelState.IsValid)
            {
                errors = null;

                return true;
            }

            errors = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

            return false;
        }
    }
}
