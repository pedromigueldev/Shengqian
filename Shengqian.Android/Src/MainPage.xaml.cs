using Shengqian.Android.Controls;
using Shengqian.Android.Controls.Models;
using Shengqian.Domain.Dto;

namespace Shengqian.Android;

public partial class MainPage : ContentPage
{
	private List<CategoryCreateDto> Categories { get; init; } = [
		new("Groceries", "Supermarket", [new (1), new (2)]),
		new("Transport", "Bus pass", [new (1), new (2)]),
		new("Coffee", "Morning espresso", [new (1), new (2)]),
		new("Rent", "Monthly rent", [new (1), new (2)]),
		new("Internet", "Fiber plan", [new (1), new (2)])
	];

	private CategoryCreateDto[] Overviews { get; init; } = [new("Entradas", "ganhos mensais", [new (1000)]), new("Saídas", "gastos mensais", [new (1000)])];

	public MainPage()
	{
		InitializeComponent();

		CardList.ItemsSource = Categories.Select(ModelImpl.ToCardIitem);
		Overview.ItemsSource = Overviews.Select(ModelImpl.ToCardIitem);
	}
}
