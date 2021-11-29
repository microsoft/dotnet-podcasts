namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class MobileShell
    {
        public MobileShell()
        {
            InitializeComponent();

            BindingContext = new ShellViewModel();
        }
    }
}
