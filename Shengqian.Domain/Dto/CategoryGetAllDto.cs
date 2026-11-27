
using SmartCtor.Attribute;
using System.Text.Json.Serialization;
using Shengqian.Domain.Model;

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
            new(category.Id, category.Name, category.Description, [.. category.Transactions.Select(ToGetAllDto)]);
    }
}

[JsonSerializable(typeof(List<CategoryGetAllDto>))]
[JsonSerializable(typeof(CategoryGetAllDto))]
internal partial class SourceGenerationContext : JsonSerializerContext { }