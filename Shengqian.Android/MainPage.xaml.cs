using Shengqian.Android.Controls;
using Shengqian.Android.Controls.Models;
using Shengqian.Android.Http;
using Shengqian.Domain.Dto;

namespace Shengqian.Android;

public partial class MainPage : ContentPage
{
	readonly IRestService<CategoryGetAllDto> _restService;

	private CategoryGetAllDto[] Overviews { get; init; } = [
		new(new (0), "Entradas", "ganhos mensais", []), new(new(0), "Saídas", "gastos mensais", [])
	];

    protected async override void OnAppearing() {
        base.OnAppearing();
		await Permissions.RequestAsync<LocalNetworkPermission>();
		var data = await _restService.RefreshDataAsync();
		CardList.ItemsSource = data.Select(ModelImpl.ToCardIitem);
		Overview.ItemsSource = Overviews.Select(ModelImpl.ToCardIitem);
    }


	public MainPage(IRestService<CategoryGetAllDto> restService)
	{
		InitializeComponent();
		_restService = restService;
	}
}
