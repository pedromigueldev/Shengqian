
using System.ComponentModel.DataAnnotations;
using SmartCtor.Attribute;

namespace Shengqian.Domain;

[SmartCtor]
public partial record TransactionId([Range(minimum: 0, maximum: int.MaxValue)] int Key);

[SmartCtor]
public partial record Transaction (
    [Required] TransactionId Id,
    [Required, Range(0, 100)] decimal Amount, 
    [Required, StringLength(10, MinimumLength = 6)] string Description, 
    ICollection<Category> Categories,
    [Required] DateTime Date
);