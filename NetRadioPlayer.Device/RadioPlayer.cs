using System;
using LibVLCSharp.Shared;

namespace NetRadioPlayer.Device
{
  public class RadioPlayer : IDisposable
  {
    private readonly LibVLC lib;
    private MediaPlayer mediaPlayer;
    private Media media;

    public RadioPlayer()
    {
      lib = new LibVLC();
      mediaPlayer = new MediaPlayer(lib);

      mediaPlayer.Playing += OnPlaying;
      mediaPlayer.Paused += OnPaused;
    }

    public event PlayerEventHandler RadioPlaying;
    public event PlayerEventHandler RadioPaused;

    public delegate void PlayerEventHandler(string uri);

    public string CurrentlyPlaying { get; private set; }

    public void Play(string radioUrl)
    {
      if (String.IsNullOrEmpty(radioUrl))
      {
        throw new ArgumentNullException("radioUrl", "You have to provide a network radio URL.");
      }

      media = new Media(lib, radioUrl, FromType.FromLocation);
      mediaPlayer.Media = media;
      mediaPlayer.Play();

      CurrentlyPlaying = radioUrl;
    }

    public void Pause()
    {
      mediaPlayer.Pause();
    }

    public void Dispose()
    {
      lib.Dispose();
      mediaPlayer.Dispose();
      media.Dispose();
      Console.WriteLine("Objects disposed");
    }

    private void OnPaused(object sender, EventArgs e)
    {
      Console.WriteLine("Paused");
      RadioPaused.Invoke(String.Empty);
    }

    private void OnPlaying(object sender, EventArgs e)
    {
      Console.WriteLine("Play");
      RadioPlaying.Invoke(media.Mrl);
    }
  }
}
