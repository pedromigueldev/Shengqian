using System.ComponentModel.DataAnnotations;
using Shengqian.Domain;
using SmartCtor.Attribute;
using SmartCtor.Monadics;

namespace Shengqian.Domain.Dto;

[SmartCtor(GenerateOptional = true)]
public partial record TransactionCreateDto(
    [Required, Range(0, int.MaxValue)]
    decimal Amount, 
    [Required, StringLength(200, MinimumLength = 6)]
    string Description,
    [Required] List<CategoryId> Categories,
    DateTime TransactionDate
);

public static partial class DtoMapper
{
    extension (TransactionCreateDto transactionCreateDto)
    {
        public Result<Transaction, List<ValidationResult>> ToDomain (List<Category> category) => 
            Transaction.Create(
                new TransactionId(0),
                transactionCreateDto.Amount, 
                transactionCreateDto.Description, 
                category, 
                transactionCreateDto.TransactionDate);
    }

    extension (Transaction transaction)
    {
        public Result<TransactionCreateDto, List<ValidationResult>> ToDomainDto () =>
            TransactionCreateDto.Create(transaction.Amount, transaction.Description, [.. transaction.Categories.Select(e => e.Id)], transaction.Date);
    }
}