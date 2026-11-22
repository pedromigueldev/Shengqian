using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shengqian.AppDbContext;
using Shengqian.Domain;
using Shengqian.Dto;
using Shengqian.Extensions;

namespace Shengqian.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController(IDbContextFactory<ApplicationDbContext> dbContextFactory) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<List<TransactionCreateDto>>> Get(CancellationToken cancellationToken) {
        
        using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var list = await db.Transactions
            .ToListAsync(cancellationToken)
            .TryMapAsync(e => e.Select(e => DtoMapper.ToDomainDto(e)));

        return list
            .Match(
                value => value.Any(f => f is not TransactionCreateDto)
                    ? Problem(detail: "Server returned malformed data")
                    : Ok(value.Select(f => f.Unwrap())),
                err => (ActionResult)Results.InternalServerError(err)
            );
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Post(int id, [FromBody] TransactionCreateDtoOptional transactionCreateDtoOptional, CancellationToken cancellationToken) {
        Console.WriteLine($"Patch is {transactionCreateDtoOptional}");
        using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var stubId = TransactionId.Create(id);
        if (stubId is not TransactionId Id) return BadRequest(stubId.UnwrapErr());

        var row = await db.Transactions.FindAsync([Id], cancellationToken: cancellationToken);
        if (row is not { } Transaction) return NotFound(new { Message = $"Transaction of id {id} does not exist..."});

        Console.WriteLine($"Patch is {transactionCreateDtoOptional}");
        Console.WriteLine($"Database is {Transaction}");

        return Ok(transactionCreateDtoOptional);
    }
}