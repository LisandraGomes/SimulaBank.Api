using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimulaBank.Application.Input;

namespace SimulaBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankBoxController : ControllerBase
    {
        public BankBoxController()
        {

        }
        [HttpPost("Create")]
        [Authorize(Roles = "InvetstorCliente")]
        public async Task<IActionResult> CreateBankBox(PiggyBoxRegisterInput request)
        {
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
