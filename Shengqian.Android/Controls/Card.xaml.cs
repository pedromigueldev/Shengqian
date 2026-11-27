
namespace Shengqian.Android.Controls;

public partial class Card : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(Card), string.Empty,
        propertyChanged: (b, _, v) => ((Card)b).TitleLabel.Text = (string)v);

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
        nameof(Description), typeof(string), typeof(Card), string.Empty,
        propertyChanged: (b, _, v) => ((Card)b).DescriptionLabel.Text = (string)v);

    public static readonly BindableProperty DetailProperty = BindableProperty.Create(
        nameof(Detail), typeof(string), typeof(Card), string.Empty,
        propertyChanged: (b, _, v) => ((Card)b).DetailLabel.Text = (string)v);

    public Card()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Detail
    {
        get => (string)GetValue(DetailProperty);
        set => SetValue(DetailProperty, value);
    }
}
