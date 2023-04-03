namespace Microsoft.NetConf2021.Maui.Views;

public partial class ShowItemView
{
    public static readonly BindableProperty SubscriptionCommandProperty =
        BindableProperty.Create(
            nameof(SubscriptionCommand),
            typeof(ICommand), 
            typeof(ShowItemView), 
            default(string));

    public static readonly BindableProperty SubscriptionCommandParameterProperty =
        BindableProperty.Create(
            nameof(SubscriptionCommandParameter),
            typeof(ShowViewModel),
            typeof(ShowItemView),
            default(ShowViewModel));

    public ICommand SubscriptionCommand
    {
        get { return (ICommand)GetValue(SubscriptionCommandProperty); }
        set { SetValue(SubscriptionCommandProperty, value); }
    }

    public ShowViewModel SubscriptionCommandParameter
    {
        get { return (ShowViewModel)GetValue(SubscriptionCommandParameterProperty); }
        set { SetValue(SubscriptionCommandParameterProperty, value); }
    }

    public ShowItemView()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if(BindingContext is not ShowViewModel vm)
        {
            return;
        }

        vm.InitializeCommand.Execute(null);
    }
}

