
using System.ComponentModel.DataAnnotations;
using SmartCtor.Attribute;
using Shengqian.Domain;
using SmartCtor.Monadics;
using System.Text.Json;
using System.Text.Json.Serialization;

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

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(List<CategoryGetAllDto>))]
[JsonSerializable(typeof(CategoryGetAllDto))]
internal partial class SourceGenerationContext : JsonSerializerContext { }