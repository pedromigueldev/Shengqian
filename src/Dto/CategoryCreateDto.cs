
using System.ComponentModel.DataAnnotations;
using SmartCtor.Attribute;
using Shengqian.Domain;
using SmartCtor.Monadics;

namespace Shengqian.Dto;

[SmartCtor]
public partial record CategoryCreateDto(
    [Required, StringLength(maximumLength: 15, MinimumLength = 10)] string Name,
    [Required, StringLength(maximumLength: 100)] string Description,
    List<TransactionId> Transactions
);

public static partial class DtoMapper {
    extension (Category category) {
        public Result<CategoryCreateDto, List<ValidationResult>> ToDomainDto () =>
            CategoryCreateDto.Create(category.Name, category.Description, [.. category.Transactions.Select(e => e.Id)]);
    }
}