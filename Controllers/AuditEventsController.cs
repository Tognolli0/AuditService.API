using AuditService.API.Contracts;
using AuditService.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.API.Controllers;

[ApiController]
[Route("api/audit-events")]
public sealed class AuditEventsController(IAuditEventRepository repository) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Create([FromBody] CreateAuditEventRequest request)
    {
        try
        {
            var id = await repository.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, new
            {
                id,
                message = "Evento de auditoria registrado com sucesso."
            });
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = "Nao foi possivel acessar o banco de auditoria.",
                detail = exception.Message
            });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Get([FromQuery] AuditEventQuery query)
    {
        try
        {
            var items = await repository.GetAsync(query);
            return Ok(items);
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = "Nao foi possivel consultar o banco de auditoria.",
                detail = exception.Message
            });
        }
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var item = await repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = "Nao foi possivel consultar o banco de auditoria.",
                detail = exception.Message
            });
        }
    }

    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            var summary = await repository.GetSummaryAsync();
            return Ok(summary);
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = "Nao foi possivel gerar o resumo da auditoria.",
                detail = exception.Message
            });
        }
    }
}
