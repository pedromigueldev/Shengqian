#!/usr/env dotnet

#:sdk Microsoft.NET.Sdk
#:property TargetFramework=net11.0-android
#:property OutputType=Exe
#:property RootNamespace=Shengqian.Android
#:property UseMaui=true
#:property SingleProject=true
#:property ImplicitUsings=true
#:property WarningAsError=nullable

#:property ApplicationTitle=Shengqian.Android
#:property ApplicationId=com.companyname.shengqian.android
#:property ApplicationDisplayVersion=1.0
#:property ApplicationVersion=1
#:property SupportedOSPlatformVersion=24.0

#:package Microsoft.Maui.Controls@11.0.0
#:package Microsoft.Extensions.Logging.Debug@11.0.0
#:package Microsoft.Extensions.Configuration.Json@11.0.0

#:include Src/**/*.cs
#:include Platforms/**/*.cs
#:include ../Shengqian.Domain/Shengqian.Domain.cs

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

		AddEmbeddedJson(builder.Configuration, "appsettings.json");
		AddEmbeddedJson(builder.Configuration, "appsettings.Local.json");

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
