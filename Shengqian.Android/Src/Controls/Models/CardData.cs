namespace Shengqian.Android.Controls.Models;

using Shengqian.Domain.Dto;
public readonly record struct CardData(string Title, string Description, string Detail);

public static class ModelImpl {
    extension(CategoryGetAllDto self) {
        public CardData ToCardIitem () {
            return new CardData (self.Name, self.Description, $"{self.Transactions.Count} transactions");
        }
    }
}