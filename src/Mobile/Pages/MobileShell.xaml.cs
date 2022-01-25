namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class MobileShell
    {
        public MobileShell(ShellViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
