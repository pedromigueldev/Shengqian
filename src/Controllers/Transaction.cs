using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shengqian.AppDbContext;
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
}