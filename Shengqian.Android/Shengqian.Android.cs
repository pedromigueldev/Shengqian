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
#:property WindowsPackageType=None
#:property SupportedOSPlatformVersion=24.0

#:package Microsoft.Maui.Controls@11.0.0
#:package Microsoft.Extensions.Logging.Debug@11.0.0

#:include Src/**/*.cs

using Microsoft.Extensions.Logging;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
