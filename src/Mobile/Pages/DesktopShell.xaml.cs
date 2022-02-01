namespace Microsoft.NetConf2021.Maui.Pages
{
    public partial class DesktopShell
    {
        public DesktopShell()
        {
            InitializeComponent();

            BindingContext = new ShellViewModel();
        }
    }
}
