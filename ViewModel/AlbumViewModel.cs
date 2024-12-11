using System.Collections.ObjectModel;
using Labb2_Database.Command;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Database.ViewModel;

public class AlbumViewModel : ViewModelBase
{
    public ArtistViewModel artistViewModel { get; set; }

    public DelegateCommand AddAlbumCommand { get; set; }
    public DelegateCommand RemoveAlbumCommand { get; set; }
    public DelegateCommand UpdateAlbumCommand { get; set; }

    private string _albumTitle;

    public string AlbumTitle
    {
        get => _albumTitle;
        set
        {
            _albumTitle = value;
            RaisePropertyChanged();
            AddAlbumCommand.RaiseCanExecuteChanged();
            UpdateAlbumCommand.RaiseCanExecuteChanged();
        }
    }

    private Artist _artist;

    public Artist SelectedArtist
    {
        get => _artist;
        set
        {
            _artist = value;
            RaisePropertyChanged();
            AddAlbumCommand.RaiseCanExecuteChanged();
            UpdateAlbumCommand.RaiseCanExecuteChanged();
        }
    }

    private ObservableCollection<Artist> _artists;

    public ObservableCollection<Artist> Artists
    {
        get => _artists;
        set
        {
            _artists = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Album> _albums;

    public ObservableCollection<Album> Albums
    {
        get => _albums;
        set
        {
            _albums = value;
            RaisePropertyChanged();
        }
    }

    private Album _selectedAlbum;

    public Album SelectedAlbum
    {
        get => _selectedAlbum;
        set
        {
            _selectedAlbum = value;
            AlbumTitle = value.Title;
            RaisePropertyChanged();
            RemoveAlbumCommand.RaiseCanExecuteChanged();
        }
    }

    public AlbumViewModel(ArtistViewModel artistViewModel)
    {
        this.artistViewModel = artistViewModel;

        LoadAlbums();

        AddAlbumCommand = new DelegateCommand(AddAlbumButton, CanAddAlbumButton);
        RemoveAlbumCommand = new DelegateCommand(RemoveAlbumButton, CanRemoveAlbumButton);
        UpdateAlbumCommand = new DelegateCommand(UpdateAlbumTitleButton, CanUpdateAlbumTitleButton);
    }

    public void LoadAlbums()
    {
        using var db = new EveryloopContext();

        Albums = new ObservableCollection<Album>(db.Albums.Include(a => a.Artist).ToList());
    }

    private bool CanAddAlbumButton(object? arg)
    {
        return !string.IsNullOrEmpty(AlbumTitle) && SelectedArtist != null;
    }

    public void AddAlbumButton(Object obj)
    {
        using var db = new EveryloopContext();

        db.Albums.Add(new Album()
            { AlbumId = GetNewAlbumId(), Title = AlbumTitle, ArtistId = SelectedArtist.ArtistId });
        db.SaveChanges();
        
        LoadAlbums();
    }

    private int GetNewAlbumId()
    {
        using var db = new EveryloopContext();
        return db.Albums.Max(a => a.AlbumId) + 1;
    }

    private bool CanRemoveAlbumButton(object? arg)
    {
        return SelectedAlbum != null;
    }

    public void RemoveAlbumButton(Object obj)
    {
        using var db = new EveryloopContext();

        db.Albums.Remove(SelectedAlbum);
        db.SaveChanges();

        LoadAlbums();
    }

    private bool CanUpdateAlbumTitleButton(object? arg)
    {
        return SelectedAlbum != null && !string.IsNullOrEmpty(AlbumTitle);
    }

    public void UpdateAlbumTitleButton(Object obj)
    {
        var db = new EveryloopContext();

        var album = db.Albums.Find(SelectedAlbum.AlbumId);
        album.Title = AlbumTitle;
        album.ArtistId = SelectedArtist.ArtistId;
        db.SaveChanges();

        LoadAlbums();
    }
}