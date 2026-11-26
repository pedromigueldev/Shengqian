
using System.ComponentModel.DataAnnotations;
using SmartCtor.Attribute;
using Shengqian.Domain;
using SmartCtor.Monadics;

namespace Shengqian.Domain.Dto;

[SmartCtor]
public partial record CategoryGetAllDto(
    CategoryId Id,
    string Name,
    string Description,
    List<TransactionGetAllDto> Transactions
);

public static partial class DtoMapper {
    extension (Category category) {
        public CategoryGetAllDto ToGetAllDto () => 
            new(category.Id, category.Name, category.Description, [.. category.Transactions.Select(DtoMapper.ToGetAllDto)]);
    }
}