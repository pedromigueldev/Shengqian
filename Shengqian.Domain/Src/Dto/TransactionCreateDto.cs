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

public partial record TransactionGetAllDto(
    TransactionId Id,
    decimal Amount, 
    string Description,
    DateTime TransactionDate,
    List<Category> Categories
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
        public TransactionGetAllDto ToGetAllDto () =>
            new (transaction.Id, transaction.Amount, transaction.Description, transaction.Date, [.. transaction.Categories]);
    }
}