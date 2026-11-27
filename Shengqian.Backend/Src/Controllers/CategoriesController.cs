
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shengqian.AppDbContext;
using Shengqian.Domain;
using Shengqian.Domain.Dto;
using Shengqian.Domain.Extensions;
using Shengqian.Domain.Model;
using Shengqian.Extensions;

namespace Shengqian.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController(IDbContextFactory<ApplicationDbContext> dbContextFactory) : ControllerBase {

    [HttpGet]
    public async Task<ActionResult<List<CategoryGetAllDto>>> GerAllCategories (CancellationToken cancellationToken) {
        var res =  await dbContextFactory.CreateDbContextAsync(cancellationToken)
            .UsingAsync(async db => {
                return await db.Categories
                    .AsNoTracking()
                    .Include(e => e.Transactions)
                    .ToListAsync(cancellationToken)
                    .TryMapAsync(e => e.Select(DtoMapper.ToGetAllDto));
            });

        return res.Match(s => Ok(s), er => Problem(detail: er.Exception.Message));
    }
    
    [HttpPost]
    public async Task<ActionResult<CategoryGetAllDto>> CreateCategory([FromBody] CategoryCreateDto createDto, CancellationToken cancellationToken) {
        var stub = createDto.ToDomain();
        if (stub is not Category valid) return stub.UnwrapErr().ToValidationProblem();

        var res = await dbContextFactory
            .CreateDbContextAsync(cancellationToken)
            .UsingAsync(async db => {
                
                if (await db.Categories.AnyAsync(e => EF.Functions.Like(e.Name, $"%{valid.Name}%")))
                    return new List<ValidationResult>() {
                        new($"There is already a category with name {valid.Name}", [nameof(createDto.Name)])
                    }.ToValidationProblem();

                await db.Categories.AddAsync(valid, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                return Created(nameof(CreateCategory), new CategoryGetAllDto(valid.Id, valid.Name, valid.Description, []));
            });

        return res.Match(
                s => s, 
                err => Problem(detail: err.Exception.Message));
    }
}