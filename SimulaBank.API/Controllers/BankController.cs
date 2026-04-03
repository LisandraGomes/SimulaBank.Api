using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.API.Extension;
using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;
using System.Text.Json;

namespace SimulaBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankApplication _bankApplication;
        public BankController(IBankApplication bankApplication)
        {
            _bankApplication = bankApplication;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "InvetstorCliente")]
        [ProducesResponseType(typeof(PatternResult<PatternResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePiggy(PiggyRegisterInput request)
        {
            var cpf = User.Claims != null
                    ? JsonDocument.Parse(User.Claims.FirstOrDefault(c => c.Type == "user")?.Value).RootElement.GetProperty("Cpf").GetString()
                    : null;
            var result = await _bankApplication.CreatePiggy(request, cpf);
            return ActionResultExtension.ToActionResult(result);
        }

        [HttpGet("User/{id}/{onlyActive}")]
        [Authorize(Roles = "InvetstorCliente")]
        [ProducesResponseType(typeof(PatternResult<PatternResult<List<PiggyOutput>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPiggysByUserId([FromRoute] Guid id, bool onlyActive)
        {
            var result = await _bankApplication.GetAllPiggyByUserId(id, onlyActive);
            return ActionResultExtension.ToActionResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "InvetstorCliente")]
        [ProducesResponseType(typeof(PatternResult<PatternResult<PiggyOutput>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBankBoxById(Guid id)
        {
            var result = await _bankApplication.GetBankBoxById(id);
            return ActionResultExtension.ToActionResult(result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "InvetstorCliente")]
        [ProducesResponseType(typeof(PatternResult<PatternResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBankBox(Guid id)
        {
            var result = await _bankApplication.UpdateBankBox(id);
            return ActionResultExtension.ToActionResult(result);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "InvetstorCliente")]
        [ProducesResponseType(typeof(PatternResult<PatternResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBankBox(Guid id)
        {
            var result = await _bankApplication.DeleteBankBox(id);
            return ActionResultExtension.ToActionResult(result);
        }
    }
}
