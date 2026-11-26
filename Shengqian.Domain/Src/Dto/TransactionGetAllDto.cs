
namespace Shengqian.Domain.Dto;
public partial record TransactionGetAllDto(
    TransactionId Id,
    decimal Amount, 
    string Description,
    DateTime TransactionDate,
    List<Category> Categories
);

public static partial class DtoMapper
{
    extension (Transaction transaction)
    {
        public TransactionGetAllDto ToGetAllDto () =>
            new (transaction.Id, transaction.Amount, transaction.Description, transaction.Date, [.. transaction.Categories]);
    }
}