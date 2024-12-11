using System.Collections.ObjectModel;
using Labb2_Database.Command;
using Microsoft.EntityFrameworkCore;

namespace Labb2_Database.ViewModel;

public class EditPlaylistViewModel : ViewModelBase
{
    private PlaylistViewModel playlistViewModel;
    public DelegateCommand AddTrackToPlaylistCommand { get; set; }
    public DelegateCommand RemoveTrackFromPlaylistCommand { get; set; }

    public string currentPlaylistName { get; set; }

    public ObservableCollection<Track> TracksInPlaylist { get; set; }

    public ObservableCollection<Track> AvailableTracks { get; set; }

    private Track _selectedTrackInPlaylist;

    public Track SelectedTrackInPlaylist
    {
        get => _selectedTrackInPlaylist;
        set
        {
            _selectedTrackInPlaylist = value;
            RaisePropertyChanged();
            RemoveTrackFromPlaylistCommand.RaiseCanExecuteChanged();
            AddTrackToPlaylistCommand.RaiseCanExecuteChanged();
        }
    }

    private Track _selectedTrackInTracks;

    public Track SelectedTrackInTracks
    {
        get => _selectedTrackInTracks;
        set
        {
            _selectedTrackInTracks = value;
            RaisePropertyChanged();
            AddTrackToPlaylistCommand.RaiseCanExecuteChanged();
            RemoveTrackFromPlaylistCommand.RaiseCanExecuteChanged();
        }
    }

    public EditPlaylistViewModel(PlaylistViewModel playlistViewModel)
    {
        this.playlistViewModel = playlistViewModel;
        LoadTracks();

        currentPlaylistName = playlistViewModel.SelectedPlaylist.Name;

        AddTrackToPlaylistCommand = new DelegateCommand(AddTrackToPlaylistButton, CanAddTrackToPlaylist);
        RemoveTrackFromPlaylistCommand = new DelegateCommand(RemoveTrackFromPlaylistButton, CanRemoveTrackFromPlaylist);
    }

    public void LoadTracks()
    {
        using var db = new EveryloopContext();

        TracksInPlaylist = new ObservableCollection<Track>(
            db.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistViewModel.SelectedPlaylist.PlaylistId)
                .Select(pt => pt.Track)
                .ToList()
        );

        AvailableTracks = new ObservableCollection<Track>(db.Tracks.Where(t => !TracksInPlaylist.Contains(t)).ToList());
    }

    private bool CanAddTrackToPlaylist(object? arg)
    {
        return SelectedTrackInTracks != null;
    }

    private void AddTrackToPlaylistButton(object obj)
    {
        using var db = new EveryloopContext();

        db.PlaylistTracks.Add(new PlaylistTrack
            { PlaylistId = playlistViewModel.SelectedPlaylist.PlaylistId, TrackId = SelectedTrackInTracks.TrackId });
        db.SaveChanges();

        TracksInPlaylist.Add(SelectedTrackInTracks);
        AvailableTracks.Remove(SelectedTrackInTracks);
    }

    private bool CanRemoveTrackFromPlaylist(object? arg)
    {
        return SelectedTrackInPlaylist != null;
    }

    private void RemoveTrackFromPlaylistButton(object obj)
    {
        using var db = new EveryloopContext();

        var trackToRemove = db.PlaylistTracks.FirstOrDefault(pt =>
            pt.PlaylistId == playlistViewModel.SelectedPlaylist.PlaylistId &&
            pt.TrackId == SelectedTrackInPlaylist.TrackId);

        db.PlaylistTracks.Remove(trackToRemove);
        db.SaveChanges();

        AvailableTracks.Add(SelectedTrackInPlaylist);
        TracksInPlaylist.Remove(SelectedTrackInPlaylist);
    }
}