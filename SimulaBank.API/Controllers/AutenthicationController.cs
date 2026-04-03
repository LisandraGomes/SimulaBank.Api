using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.API.Extension;
using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;

namespace SimulaBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenthicationController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;
        public AutenthicationController(IAuthApplication authApplication)
        {
            _authApplication = authApplication;
        }

        [AllowAnonymous]
        [HttpPost()]
        [ProducesResponseType(typeof(PatternResult<PatternResult<AuthOutput>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(AuthUserInput request)
        {
            var result = await _authApplication.Login(request);
            return ActionResultExtension.ToActionResult(result);
        }
    }
}
