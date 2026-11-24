# Shengqian.Android

The mobile app for [Shengqian](../README.md): a .NET MAUI Android app built as a C# file-based app, sharing its domain layer with the backend through `Shengqian.Domain`.

## Structure

- `Shengqian.Android.cs`: entry point and project config (`#:` directives)
- `Src/`: app sources, XAML pages, resources and platform code

## Running

```sh
dotnet run Shengqian.Android.cs
```

## Building and running

```sh
dotnet build -t:Run .\Shengqian.Android\Shengqian.Android.cs
```
