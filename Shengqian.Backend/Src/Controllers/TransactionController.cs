using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shengqian.AppDbContext;
using Shengqian.Domain.Dto;
using Shengqian.Domain.Extensions;
using Shengqian.Domain.Model;
using Shengqian.Extensions;
using SmartCtor.Monadics;

namespace Shengqian.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController(IDbContextFactory<ApplicationDbContext> dbContextFactory) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<List<TransactionGetAllDto>>> Get(CancellationToken cancellationToken) {
        var res = await dbContextFactory.CreateDbContextAsync(cancellationToken)
        .UsingAsync(async db => {
            return await db.Transactions
                .Include(e => e.Categories)
                .ToListAsync(cancellationToken)
                .TryMapAsync(e => e.Select(e => DtoMapper.ToGetAllDto(e)));
        });

        return res.Match(
            s => Ok(s),
            e => Problem(detail: res.UnwrapErr().Exception.Message)
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionGetAllDto>> GetById(int id, CancellationToken cancellationToken) {
        var res = await dbContextFactory.CreateDbContextAsync(cancellationToken)
        .UsingAsync(async db => {
            var stubId = TransactionId.Create(id);
            if (stubId is not TransactionId Id) return BadRequest(stubId.UnwrapErr());

            var row = await db.Transactions.AsNoTracking()
                .Where(e => e.Id == Id)
                .Include(e => e.Categories)
                .FirstOrDefaultAsync(cancellationToken);

            if (row is null) return NotFound(new ValidationResult($"Transaction of id {id} does not exist...", [nameof(id)]));

            return (ActionResult)Ok(row.ToGetAllDto());
        });

        return res.Match(s => s, e => Problem(detail: e.Exception.Message));
    }

    [HttpPost]
    public async Task<ActionResult<TransactionCreateDto>> Post([FromBody] TransactionCreateDto tdto, CancellationToken cancellationToken) {
        var res = await dbContextFactory.CreateDbContextAsync(cancellationToken)
        .UsingAsync(async db => {
            var categoryIds = tdto.Categories.Distinct().ToList();
            var categories = await db.Categories
                .Where(e => categoryIds.Contains(e.Id))
                .ToListAsync(cancellationToken);

            var missing = categoryIds.Except(categories.Select(e => e.Id)).Select(e => e.Key).ToList();
            if (missing.Count > 0)
                return NotFound(new ValidationResult($"Categories of id {string.Join(", ", missing)} do not exist...", [nameof(tdto.Categories)]));

            var created = tdto.ToDomain(categories);
            if (created is not Transaction transaction) return BadRequest(created.UnwrapErr());

            db.Transactions.Add(transaction);
            await db.SaveChangesAsync(cancellationToken);
            return (ActionResult)Created(nameof(Get), transaction.ToGetAllDto());
        });

        return res.Match(s => s, e => Problem(detail: e.Exception.Message));
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(int id, [FromBody] TransactionCreateDtoOptional tdto, CancellationToken cancellationToken) {
        var res = await dbContextFactory.CreateDbContextAsync(cancellationToken)
        .UsingAsync(async db => {

            var stubId = TransactionId.Create(id);
            if (stubId is not TransactionId Id) return BadRequest(stubId.UnwrapErr());

            var row = await db.Transactions
                .Where(e => e.Id == Id)
                .Include(e => e.Categories)
                .FirstOrDefaultAsync(cancellationToken);

            if (row is null) return NotFound(new ValidationResult($"Transaction of id {id} does not exist...", [nameof(id)]));

            // null => field not sent, keep current categories; [] => explicitly cleared
            List<CategoryId>? categoryIds = tdto.Categories switch {
                Option<List<CategoryId>> op => op switch {
                    List<CategoryId> list => [.. list.Distinct()],
                    None => []
                },
                None => null
            };

            ICollection<Category> categories = row.Categories;
            if (categoryIds is not null) {
                categories = await db.Categories
                    .Where(e => categoryIds.Contains(e.Id))
                    .ToListAsync(cancellationToken);

                var missing = categoryIds.Except(categories.Select(e => e.Id)).Select(e => e.Key).ToList();
                if (missing.Count > 0)
                    return NotFound(new ValidationResult($"Categories of id {string.Join(", ", missing)} do not exist...", [nameof(tdto.Categories)]));
            }

            var patched = row.CreatePathced(new None(), tdto.Amount, tdto.Description, new(categories), tdto.TransactionDate);
            if (patched is List<ValidationResult> validationResults) return validationResults.ToValidationProblem();
            var transaction = patched.Unwrap();

            db.UpdateImmutable(transaction);
            await db.SaveChangesAsync(cancellationToken);
            return Ok(row.ToGetAllDto());
        });

        return res.Match(
            e => e,
            e => Problem(detail: e.Exception.Message)
        );
    }
}