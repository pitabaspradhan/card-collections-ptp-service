using CardCollections.Application.DTOs;
using CardCollections.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardCollections.Api.Controllers;

[ApiController]
[Route("api/cases")]
public class CasesController : ControllerBase
{
    private readonly ICollectionCaseService _caseService;
    private readonly IPromiseToPayService _promiseToPayService;

    public CasesController(
        ICollectionCaseService caseService,
        IPromiseToPayService promiseToPayService)
    {
        _caseService = caseService;
        _promiseToPayService = promiseToPayService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CaseResponse>> GetCase(
    Guid id)
    {
        var response =
            await _caseService.GetCaseAsync(id);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CaseResponse>> CreateCase(
    CreateCaseRequest request)
    {
        var response =
            await _caseService.CreateCaseAsync(request);

        return CreatedAtAction(
            nameof(GetCase),
            new { id = response.CaseId },
            response);
    }

    [HttpPost("{id:guid}/promise-to-pay")]
    public async Task<ActionResult<PromiseToPayResponse>>
    CreatePromiseToPay(
    Guid id,

    [FromHeader(Name = "Idempotency-Key")]
    string idempotencyKey,

    [FromHeader(Name = "X-Correlation-Id")]
    string correlationId,

    [FromBody] CreatePromiseToPayRequest request)
    {

        var response =
            await _promiseToPayService.CreatePromiseToPayAsync(
                id,
                request.Amount,
                request.PromiseDate,
                request.AgentId,
                correlationId,
                idempotencyKey);

        return Ok(response);
    }
}