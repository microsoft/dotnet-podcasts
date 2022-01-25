namespace Microsoft.NetConf2021.Maui.Pages;

public partial class CategoriesPage : BaseContentPage
{ 
    public CategoriesPage(CategoriesViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}
