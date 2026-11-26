using Shengqian.Android.Controls;
using Shengqian.Android.Controls.Models;
using Shengqian.Domain.Dto;

namespace Shengqian.Android;

public partial class MainPage : ContentPage
{
	private List<CategoryCreateDto> Categories { get; init; } = [
		new("Groceries asd", "Supermarket"),
		new("Transport", "Bus pass"),
		new("Coffee", "Morning espresso"),
		new("Rent", "Monthly rent"),
		new("Internet", "Fiber plan")
	];

	private CategoryCreateDto[] Overviews { get; init; } = [new("Entradas", "ganhos mensais"), new("Saídas", "gastos mensais")];

	public MainPage()
	{
		InitializeComponent();

		CardList.ItemsSource = Categories.Select(ModelImpl.ToCardIitem);
		Overview.ItemsSource = Overviews.Select(ModelImpl.ToCardIitem);
	}
}
