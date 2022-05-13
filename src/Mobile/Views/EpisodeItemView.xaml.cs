namespace Microsoft.NetConf2021.Maui.Views;

public partial class EpisodeItemView 
{
    public static readonly BindableProperty RemoveListenLaterCommandProperty =
    BindableProperty.Create(
        nameof(RemoveListenLaterCommand),
        typeof(ICommand),
        typeof(EpisodeItemView),
        default(string));

    public static readonly BindableProperty RemoveListenLaterCommandParameterProperty =
        BindableProperty.Create(
            nameof(RemoveListenLaterCommandParameter),
            typeof(EpisodeViewModel),
            typeof(ShowItemView),
            default(EpisodeViewModel));

    public ICommand RemoveListenLaterCommand
    {
        get { return (ICommand)GetValue(RemoveListenLaterCommandProperty); }
        set { SetValue(RemoveListenLaterCommandProperty, value); }
    }

    public EpisodeViewModel RemoveListenLaterCommandParameter
    {
        get { return (EpisodeViewModel)GetValue(RemoveListenLaterCommandParameterProperty); }
        set { SetValue(RemoveListenLaterCommandParameterProperty, value); }
    }

    public EpisodeItemView()
    {
        InitializeComponent();
    }
}
