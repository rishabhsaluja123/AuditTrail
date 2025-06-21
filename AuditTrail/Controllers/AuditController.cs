using AuditTrail.Interfaces.IServices;
using AuditTrail.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AuditTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogAudit<T>([FromBody] AuditRequest<T> request)
        {
            try
            {
                await _auditService.LogAuditAsync(
                    request.OldObject,
                    request.NewObject,
                    request.Action,
                    request.UserId,
                    request.EntityName);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log exception (using ILogger in production)
                return StatusCode(500, "An error occurred while logging audit trail");
            }
        }

        [HttpGet("{entityName}")]
        public async Task<IActionResult> GetAuditTrail(string entityName, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var auditTrail = await _auditService.GetAuditTrailAsync(entityName, page, pageSize);
                return Ok(auditTrail);
            }
            catch (Exception ex)
            {
                // Log exception (using ILogger in production)
                return StatusCode(500, "An error occurred while retrieving audit trail");
            }
        }
    }
}