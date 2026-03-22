using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;

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
        public async Task<IActionResult> CreatePiggy(PiggyRegisterInput request)
        {
            var cpf = User.Claims.FirstOrDefault(c => c.Type == "cpf")?.Value;
            var result = await _bankApplication.CreatePiggy(request, cpf);
            return Ok();
        }

        [HttpGet("/User/{id}")]
        [Authorize(Roles = "InvetstorCliente")]
        public async Task<IActionResult> GetPiggysByUserId([FromRoute] Guid id)
        {
            var result = await _bankApplication.GetAllPiggyByUserId(id);
            return Ok();
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetBankBoxById(Guid id)
        {
            return Ok();
        }
        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateBankBox(Guid id)
        {
            return Ok();
        }
        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteBankBox(Guid id)
        {
            return Ok();
        }
    }
}
