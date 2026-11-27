using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shengqian.Android.Http;
using Shengqian.Domain.Dto;

namespace Shengqian.Android;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var apiBaseUrl = builder.Configuration["Api:BaseUrl"]
			?? throw new InvalidOperationException("Api:BaseUrl is missing from appsettings.json");

		builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + "/") });
		builder.Services.AddSingleton<IRestService<CategoryGetAllDto>, CategoriesService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	static void AddEmbeddedJson(ConfigurationManager configuration, string name)
	{
		var stream = typeof(MauiProgram).Assembly.GetManifestResourceStream(name);
		if (stream is not null) configuration.AddJsonStream(stream);
	}
}
