using Plugin.Maui.Audio;
namespace MusicPlayer;



public partial class MainPage : ContentPage
{
    private readonly IAudioManager audioManager;
    IAudioPlayer player;
    int count = 0;
    private string currentSong;
    private bool isPaused = false;
    private bool isPlaying = false;
    List<string> musicFiles = new List<string>();
    List<FileResult> files = new List<FileResult>();

    public MainPage(IAudioManager audioManager)
	{
		InitializeComponent();
        this.audioManager = audioManager;
    }

    private async void SelectClicked(object sender, EventArgs e)
    {
        var results = await FilePicker.PickMultipleAsync(new PickOptions
        {
            
        });

        foreach (var result in results)
        {
            musicFiles.Add(result.FileName.ToString());
            files.Add(result);

        }

       
        listView.ItemsSource = musicFiles;

        listView.SelectedItem = musicFiles[count];
        
    }

    private void PreviousSong(object sender, EventArgs e)
    {
        if (count >= 0)
        {
            if (isPlaying == true || isPaused == true)
            {
                player.Pause();
                isPlaying = false;
                isPaused = false;
            }
            count--;
            listView.SelectedItem = musicFiles[count];
            PlayClicked(sender, e);
        }
    }

    private void NextSong(object sender, EventArgs e)
    {
        if (count <= musicFiles.Count)
        {
            if (isPlaying == true || isPaused == true)
            {
                player.Pause();
                isPlaying = false;
                isPaused = false;

            }
            count++;
            listView.SelectedItem = musicFiles[count];
            PlayClicked(sender, e);
        }
    }

    private async void PlayClicked(object sender, EventArgs e)
	{
       
        if (isPaused == true)
        {
            player.Play();
            isPlaying = true;
            isPaused = false;
        }
        else if(isPlaying == false)
        {
            player = audioManager.CreatePlayer(await files.ElementAt(count).OpenReadAsync());
            player.Play();
            isPlaying = true;
            isPaused = false;
        }

        else if(isPlaying == true)
        {
            player.Pause();
            isPlaying = false;
            isPaused = true;
        }

        player.PlaybackEnded += Player_PlaybackEnded;

    }

    private void Player_PlaybackEnded(object sender, EventArgs e)
    {
        count++;

        listView.SelectedItem = musicFiles[count];
        PlayClicked(sender, e);
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        player.Volume = value;
    }

    void listView_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {

        string fileName = listView.SelectedItem.ToString();
        int index = musicFiles.IndexOf(fileName);
        count = index;

        if (isPlaying == true || isPaused == true)
        {
            player.Pause();
            isPlaying = false;

        }
        PlayClicked(sender, e);
    }

}


