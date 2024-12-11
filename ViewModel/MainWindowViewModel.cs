namespace Labb2_Database.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public PlaylistViewModel PlaylistViewModel { get; set; }
    public ArtistViewModel ArtistViewModel { get; set; }
    public AlbumViewModel AlbumViewModel { get; set; }
    public TrackViewModel TrackViewModel { get; set; }

    public MainWindowViewModel()
    {
        PlaylistViewModel = new PlaylistViewModel();
        ArtistViewModel = new ArtistViewModel();
        AlbumViewModel = new AlbumViewModel(ArtistViewModel);
        TrackViewModel = new TrackViewModel(AlbumViewModel);
    }
}