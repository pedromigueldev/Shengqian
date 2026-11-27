
using System.ComponentModel.DataAnnotations;
using SmartCtor.Attribute;
using SmartCtor.Monadics;
using Shengqian.Domain.Model;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Shengqian.Domain.Dto;

[SmartCtor]
public partial record CategoryCreateDto(
    [Required(AllowEmptyStrings = false), StringLength(maximumLength: 15, MinimumLength = 5)] 
    string Name,
    [Required(AllowEmptyStrings = false), StringLength(maximumLength: 100)] 
    string Description
);

public static partial class DtoMapper {
    extension (Category category) {
        public Result<CategoryCreateDto, List<ValidationResult>> ToDomainDto () =>
            CategoryCreateDto.Create(category.Name, category.Description);
    }

    extension (CategoryCreateDto category) {
        public Result<Category, List<ValidationResult>> ToDomain () =>
            Category.Create(new (0), category.Name, category.Description, []);
    }
}