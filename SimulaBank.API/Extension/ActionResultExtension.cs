using Microsoft.AspNetCore.Mvc;
using SimulaBank.Application.Outputs;

namespace SimulaBank.API.Extension
{
    public static class ActionResultExtension
    {
        public static IActionResult ToActionResult<T>(this PatternResult<T> result)
        {
            return new ObjectResult(result)
            {
                StatusCode = (int)result.StatusCode
            };

        }
    }
}
