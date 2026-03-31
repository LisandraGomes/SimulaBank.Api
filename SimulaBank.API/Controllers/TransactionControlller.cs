using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.API.Extension;
using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;

namespace SimulaBank.API.Controllers
{
    /// <summary>
    /// Controlador responsável por operações de transações.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionApplication _application;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="TransactionControlller"/>.
        /// </summary>
        /// <param name="transactionApplication">Serviço de aplicação de transações.</param>
        public TransactionController(ITransactionApplication transactionApplication)
        {
            _application = transactionApplication;
        }

        /// <summary>
        /// Cria uma nova transação.
        /// </summary>
        /// <param name="request">Dados da transação.</param>
        /// <returns>Resultado da criação da transação.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(PatternResult<TransactionOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionInput request)
        {
            var result = await _application.Create(request);
            return ActionResultExtension.ToActionResult(result);
        }

        /// <summary>
        /// Obtém todas as transações de um usuário.
        /// </summary>
        /// <param name="idUser">Identificador do usuário.</param>
        /// <returns>Lista de transações do usuário.</returns>
        [HttpGet("User/{idUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByUser([FromRoute] Guid idUser)
        {
            var result = await _application.GetByUser(idUser);
            return ActionResultExtension.ToActionResult(result);
        }
    }
}