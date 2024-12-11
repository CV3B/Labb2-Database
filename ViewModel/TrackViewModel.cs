using System.Collections.ObjectModel;
using Labb2_Database.Command;

namespace Labb2_Database.ViewModel;

public class TrackViewModel : ViewModelBase
{
    public AlbumViewModel albumViewModel { get; set; }

    public DelegateCommand AddTrackCommand { get; set; }
    public DelegateCommand RemoveTrackCommand { get; set; }
    public DelegateCommand UpdateTrackCommand { get; set; }

    private int _trackLength;

    public int TrackLength
    {
        get => _trackLength;
        set
        {
            _trackLength = value;
            RaisePropertyChanged();
        }
    }

    private string _trackName;

    public string TrackName
    {
        get => _trackName;
        set
        {
            _trackName = value;
            RaisePropertyChanged();
            AddTrackCommand.RaiseCanExecuteChanged();
            UpdateTrackCommand.RaiseCanExecuteChanged();
        }
    }

    private Genre _selectedGenre;

    public Genre SelectedGenre
    {
        get => _selectedGenre;
        set
        {
            _selectedGenre = value;
            RaisePropertyChanged();
            AddTrackCommand.RaiseCanExecuteChanged();
            UpdateTrackCommand.RaiseCanExecuteChanged();
        }
    }

    private Track _selectedTrack;

    public Track SelectedTrack
    {
        get => _selectedTrack;
        set
        {
            _selectedTrack = value;
            TrackName = value.Name;
            TrackLength = value.Milliseconds;
            SelectedAlbum = albumViewModel.Albums.FirstOrDefault(a => a.AlbumId == SelectedTrack.AlbumId);
            SelectedGenre = Genres.FirstOrDefault(g => g.GenreId == SelectedTrack.GenreId);
            RaisePropertyChanged();
            RemoveTrackCommand.RaiseCanExecuteChanged();
        }
    }

    private Album _selectedAlbum;

    public Album SelectedAlbum
    {
        get => _selectedAlbum;
        set
        {
            _selectedAlbum = value;
            RaisePropertyChanged();
            AddTrackCommand.RaiseCanExecuteChanged();
            UpdateTrackCommand.RaiseCanExecuteChanged();
        }
    }

    private ObservableCollection<Track> _tracks;

    public ObservableCollection<Track> Tracks
    {
        get => _tracks;
        set
        {
            _tracks = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Genre> _genres;

    public ObservableCollection<Genre> Genres
    {
        get => _genres;
        set
        {
            _genres = value;
            RaisePropertyChanged();
        }
    }

    public TrackViewModel(AlbumViewModel albumViewModel)
    {
        this.albumViewModel = albumViewModel;

        LoadTracks();
        LoadGenres();

        AddTrackCommand = new DelegateCommand(AddTrackButton, CanAddTrackButton);
        RemoveTrackCommand = new DelegateCommand(RemoveTrackButton, CanRemoveTrackButton);
        UpdateTrackCommand = new DelegateCommand(UpdateTrackNameButton, CanUpdateTrackNameButton);
    }

    public void LoadTracks()
    {
        using var db = new EveryloopContext();

        Tracks = new ObservableCollection<Track>(db.Tracks.ToList());
    }

    public void LoadGenres()
    {
        using var db = new EveryloopContext();

        Genres = new ObservableCollection<Genre>(db.Genres.ToList());
    }

    private bool CanAddTrackButton(object? arg)
    {
        return !string.IsNullOrEmpty(TrackName) && SelectedAlbum != null && SelectedGenre != null;
    }

    public void AddTrackButton(Object obj)
    {
        using var db = new EveryloopContext();

        db.Tracks.Add(new Track()
        {
            TrackId = GetNewTrackId(), Name = TrackName, AlbumId = SelectedAlbum.AlbumId, Milliseconds = TrackLength,
            GenreId = SelectedGenre.GenreId, MediaTypeId = 1
        });
        db.SaveChanges();
        
        LoadTracks();
    }

    private int GetNewTrackId()
    {
        using var db = new EveryloopContext();
        return db.Tracks.Max(t => t.TrackId) + 1;
    }

    private bool CanRemoveTrackButton(object? arg)
    {
        return SelectedTrack != null;
    }

    public void RemoveTrackButton(Object obj)
    {
        using var db = new EveryloopContext();

        db.Tracks.Remove(SelectedTrack);
        db.SaveChanges();

        LoadTracks();
    }

    private bool CanUpdateTrackNameButton(object? arg)
    {
        return SelectedTrack != null && !string.IsNullOrEmpty(TrackName);
    }

    public void UpdateTrackNameButton(Object obj)
    {
        var db = new EveryloopContext();

        var track = db.Tracks.Find(SelectedTrack.TrackId);
        track.Name = TrackName;
        track.AlbumId = SelectedAlbum.AlbumId;
        track.Milliseconds = TrackLength;
        track.GenreId = SelectedGenre.GenreId;
        db.SaveChanges();

        LoadTracks();
    }
}