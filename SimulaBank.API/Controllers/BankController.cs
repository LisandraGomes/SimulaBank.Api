using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;

namespace SimulaBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankController : ControllerBase
    {
        private readonly IBankApplication _bankApplication;
        public BankController(IBankApplication bankApplication)
        {
            _bankApplication = bankApplication;
        }
        [HttpPost("Create")]
        [Authorize(Roles = "InvetstorCliente")]
        public async Task<IActionResult> CreateBankBox(PiggyBoxRegisterInput request)
        {
            //var result = await _bankApplication.CreateBankBox(request);
            return Ok();
        }

        [HttpGet("/User/{id}")]
        public async Task<IActionResult> GetBankBox([FromRoute] Guid id)
        {
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
