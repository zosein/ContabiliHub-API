using Microsoft.AspNetCore.Mvc;

namespace ContabiliHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<object> GetHealth()
        {
            return Ok(new
            {
                status = "✅ API Funcionando",
                timestamp = DateTime.UtcNow,
                version = "Refatorado - Records + Validação Nativa",
                features = new[]
                {
                    "✅ Records nativos (sem AutoMapper)",
                    "✅ Validação nativa (sem FluentValidation)",
                    "✅ JWT Authentication",
                    "✅ EF Core + SQL Server",
                    "✅ Clean Architecture"
                }
            });
        }
    }
}