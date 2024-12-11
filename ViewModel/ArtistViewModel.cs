using System.Collections.ObjectModel;
using Labb2_Database.Command;

namespace Labb2_Database.ViewModel;

public class ArtistViewModel : ViewModelBase
{
    public DelegateCommand AddArtistCommand { get; set; }
    public DelegateCommand RemoveArtistCommand { get; set; }
    public DelegateCommand UpdateArtistNameCommand { get; set; }

    private string _newArtistName;

    public string NewArtistName
    {
        get => _newArtistName;
        set
        {
            _newArtistName = value;
            RaisePropertyChanged();
            AddArtistCommand.RaiseCanExecuteChanged();
            UpdateArtistNameCommand.RaiseCanExecuteChanged();
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

    private Artist _selectedArtist;

    public Artist SelectedArtist
    {
        get => _selectedArtist;
        set
        {
            _selectedArtist = value;
            NewArtistName = value.Name;
            RaisePropertyChanged();
            RemoveArtistCommand.RaiseCanExecuteChanged();
        }
    }

    public ArtistViewModel()
    {
        LoadArtists();

        AddArtistCommand = new DelegateCommand(AddArtistButton, CanAddArtistButton);
        RemoveArtistCommand = new DelegateCommand(RemoveArtistButton, CanRemoveArtistButton);
        UpdateArtistNameCommand = new DelegateCommand(UpdateArtistNameButton, CanUpdateArtistNameButton);
    }

    public void LoadArtists()
    {
        using var db = new EveryloopContext();

        Artists = new ObservableCollection<Artist>(db.Artists.ToList());
    }

    private bool CanAddArtistButton(object? arg)
    {
        return !string.IsNullOrEmpty(NewArtistName);
    }

    public void AddArtistButton(Object obj)
    {
        using var db = new EveryloopContext();

        db.Artists.Add(new Artist() { ArtistId = GetNewArtistId(), Name = NewArtistName });
        db.SaveChanges();
        
        LoadArtists();
    }

    private int GetNewArtistId()
    {
        using var db = new EveryloopContext();
        return db.Artists.Max(a => a.ArtistId) + 1;
    }

    private bool CanRemoveArtistButton(object? arg)
    {
        return SelectedArtist != null;
    }

    public void RemoveArtistButton(Object obj)
    {
        using var db = new EveryloopContext();

        db.Artists.Remove(SelectedArtist);
        db.SaveChanges();

        LoadArtists();
    }

    private bool CanUpdateArtistNameButton(object? arg)
    {
        return SelectedArtist != null && !string.IsNullOrEmpty(NewArtistName);
    }

    public void UpdateArtistNameButton(Object obj)
    {
        var db = new EveryloopContext();

        var artist = db.Artists.Find(SelectedArtist.ArtistId);

        artist.Name = NewArtistName;
        db.SaveChanges();

        LoadArtists();
    }
}