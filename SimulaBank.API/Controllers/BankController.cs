using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.API.Extension;
using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
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
        public async Task<IActionResult> GetPiggysByUserId([FromRoute] Guid id, bool onlyActive)
        {
            var result = await _bankApplication.GetAllPiggyByUserId(id, onlyActive);
            return ActionResultExtension.ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBankBoxById(Guid id)
        {
            var result = await _bankApplication.GetBankBoxById(id);
            return ActionResultExtension.ToActionResult(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBankBox(Guid id)
        {
            var result = await _bankApplication.UpdateBankBox(id);
            return ActionResultExtension.ToActionResult(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankBox(Guid id)
        {
            var result = await _bankApplication.DeleteBankBox(id);
            return ActionResultExtension.ToActionResult(result);
        }
    }
}
