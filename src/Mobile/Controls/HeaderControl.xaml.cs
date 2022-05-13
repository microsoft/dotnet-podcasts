namespace Microsoft.NetConf2021.Maui.Controls;

public partial class HeaderControl : ContentView
{
    public static readonly BindableProperty SearchCommandProperty =
        BindableProperty.Create(
            nameof(SearchCommand), 
            typeof(ICommand), 
            typeof(HeaderControl), 
            null);

    public static readonly BindableProperty TextToSearchProperty =
        BindableProperty.Create(
            nameof(TextToSearch),
            typeof(string),
            typeof(HeaderControl),
            string.Empty);

    public static readonly BindableProperty ShowSearchCategoriesProperty =
        BindableProperty.Create(
            nameof(ShowSearchCategories),
            typeof(bool),
            typeof(HeaderControl),
            true);

    public bool ShowSearchCategories
    {
        get { return (bool)GetValue(ShowSearchCategoriesProperty);}
        set { SetValue(ShowSearchCategoriesProperty, value); }
    }

    public ICommand SearchCommand
    {
        get { return (ICommand)GetValue(SearchCommandProperty); }
        set {  SetValue(SearchCommandProperty, value); }
    }

    public string TextToSearch
    {
        get { return (string)GetValue(TextToSearchProperty); }
        set { SetValue(TextToSearchProperty, value); }
    }

    public HeaderControl()
    {
        AutomationProperties.SetIsInAccessibleTree(this, true);
        InitializeComponent();
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(CategoriesPage)}");
    }
}

