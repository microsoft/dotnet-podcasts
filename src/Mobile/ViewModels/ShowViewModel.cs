using MvvmHelpers.Interfaces;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public class ShowViewModel : ObservableObject
{
    public Show Show { get; set; }

    private bool isSuscribed;

    public bool IsSubscribed 
    { 
        get => isSuscribed;
        set
        {
            isSuscribed = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Episode> Episodes { get => Show?.Episodes; }

    public Uri Image { get => Show?.Image; }

    public string Author { get => Show?.Author; }

    public string Title { get => Show?.Title; }

    public string Description { get => Show?.Description; }

    public IAsyncCommand NavigateToDetailCommand { get; set; }

    public ShowViewModel(Show show, bool isSubscribed)
    {
        Show = show;
        NavigateToDetailCommand = new AsyncCommand(NavigateToDetailCommandExecute);
        isSuscribed = isSubscribed;
    }

    private Task NavigateToDetailCommandExecute() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={Show.Id}");
}
