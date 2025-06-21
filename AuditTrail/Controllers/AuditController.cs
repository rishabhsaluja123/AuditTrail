using Audit.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Audit.Interfaces.IServices;

namespace Audit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditController> _logger;

        public AuditController(IAuditService auditService, ILogger<AuditController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new audit  entry
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AuditResponse>> CreateAudit([FromBody] AuditRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _auditService.CreateAuditAsync(request);

                _logger.LogInformation("Audit  created for entity {EntityName} ",
                    request.EntityName, request.UserId);

                return CreatedAtAction(nameof(GetAudit), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating audit  for entity {EntityName}", request.EntityName);
                return StatusCode(500, "An error occurred while creating the audit ");
            }
        }

        /// <summary>
        /// Gets audit s with optional filtering and pagination
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResult<AuditResponse>>> GetAudits([FromQuery] AuditQueryRequest query)
        {
            try
            {
                var result = await _auditService.GetAuditsAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit s");
                return StatusCode(500, "An error occurred while retrieving audit s");
            }
        }

        /// <summary>
        /// Gets a specific audit  by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditResponse>> GetAudit(int id)
        {
            try
            {
                var result = await _auditService.GetAuditByIdAsync(id);

                if (result == null)
                {
                    return NotFound($"Audit  with ID {id} not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit  with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the audit ");
            }
        }
    }
}