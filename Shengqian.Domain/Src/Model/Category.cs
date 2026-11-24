using System.ComponentModel.DataAnnotations;
using SmartCtor.Attribute;

namespace Shengqian.Domain;

[SmartCtor]
public partial record CategoryId([Range(minimum: 0, maximum: int.MaxValue)] int Key);

[SmartCtor]
public partial record Category(
    [Required] CategoryId Id,
    [Required, StringLength(maximumLength: 40, MinimumLength = 5)] string Name,
    [Required, StringLength(maximumLength: 100)] string Description,
    ICollection<Transaction> Transactions
);