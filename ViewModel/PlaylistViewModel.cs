using System.Collections.ObjectModel;
using Labb2_Database.Command;

namespace Labb2_Database.ViewModel;

public class PlaylistViewModel : ViewModelBase
{
    private MainWindowViewModel mainWindowViewModel;

    private EditPlaylistView editPlaylistView;
    public DelegateCommand AddPlaylistCommand { get; set; }
    public DelegateCommand RemovePlaylistCommand { get; set; }
    public DelegateCommand UpdatePlaylistNameCommand { get; set; }
    public DelegateCommand ConfigurePlaylistCommand { get; set; }

    private string _newPlaylistName;

    public string NewPlaylistName
    {
        get => _newPlaylistName;
        set
        {
            _newPlaylistName = value;
            RaisePropertyChanged();
            AddPlaylistCommand.RaiseCanExecuteChanged();
            UpdatePlaylistNameCommand.RaiseCanExecuteChanged();
            ConfigurePlaylistCommand.RaiseCanExecuteChanged();
        }
    }

    private ObservableCollection<Playlist> _playlists;

    public ObservableCollection<Playlist> Playlists
    {
        get => _playlists;
        set
        {
            _playlists = value;
            RaisePropertyChanged();
        }
    }

    private Playlist _selectedPlaylist;

    public Playlist SelectedPlaylist
    {
        get => _selectedPlaylist;
        set
        {
            _selectedPlaylist = value;
            NewPlaylistName = value.Name;
            RaisePropertyChanged();
            RemovePlaylistCommand.RaiseCanExecuteChanged();
        }
    }

    public PlaylistViewModel()
    {
        LoadPlaylists();

        AddPlaylistCommand = new DelegateCommand(AddPlaylistButton, CanAddPlaylistButton);
        RemovePlaylistCommand = new DelegateCommand(RemovePlaylistButton, CanRemovePlaylistButton);
        UpdatePlaylistNameCommand = new DelegateCommand(UpdatePlaylistNameButton, CanUpdatePlaylistName);
        ConfigurePlaylistCommand = new DelegateCommand(ConfigurePlaylistButton, CanConfigurePlaylistButton);
    }


    public void LoadPlaylists()
    {
        using var db = new EveryloopContext();

        Playlists = new ObservableCollection<Playlist>(db.Playlists.ToList());
    }

    private bool CanAddPlaylistButton(object? arg)
    {
        return !string.IsNullOrWhiteSpace(NewPlaylistName);
    }

    private void AddPlaylistButton(object obj)
    {
        using var db = new EveryloopContext();

        db.Playlists.Add(new Playlist { PlaylistId = GetNewPlaylistId(), Name = NewPlaylistName });
        db.SaveChanges();

        NewPlaylistName = "";
        LoadPlaylists();
    }

    private int GetNewPlaylistId()
    {
        using var db = new EveryloopContext();
        return db.Playlists.Max(p => p.PlaylistId) + 1;
    }

    private bool CanRemovePlaylistButton(object? arg)
    {
        return SelectedPlaylist != null;
    }

    private void RemovePlaylistButton(object obj)
    {
        using var db = new EveryloopContext();

        db.Playlists.Remove(SelectedPlaylist);
        db.SaveChanges();

        LoadPlaylists();
    }

    private bool CanUpdatePlaylistName(object? arg)
    {
        return SelectedPlaylist != null && !string.IsNullOrWhiteSpace(NewPlaylistName);
    }

    private void UpdatePlaylistNameButton(object obj)
    {
        using var db = new EveryloopContext();

        var playlist = db.Playlists.Find(SelectedPlaylist.PlaylistId);

        playlist.Name = NewPlaylistName;
        db.SaveChanges();

        LoadPlaylists();
    }

    private bool CanConfigurePlaylistButton(object? arg)
    {
        return SelectedPlaylist != null;
    }

    private void ConfigurePlaylistButton(object obj)
    {
        EditPlaylistViewModel editPlaylistViewModel = new EditPlaylistViewModel(this);
        editPlaylistView = new EditPlaylistView() { DataContext = editPlaylistViewModel };

        editPlaylistView.ShowDialog();
    }
}