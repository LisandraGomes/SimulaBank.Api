using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.API.Extension;
using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;

namespace SimulaBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionControlller
    {
        private readonly ITransactionApplication _application;
        public TransactionControlller(ITransactionApplication transactionApplication)
        {
            _application = transactionApplication;
        }
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionInput request)
        {
            var result = await _application.Create(request);
            return ActionResultExtension.ToActionResult(result);
        }
    }
}
