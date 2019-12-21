using System;
using LibVLCSharp.Shared;

namespace NetRadioPlayer.Device
{
  public class RadioPlayer : IDisposable
  {
    public event EventHandler RadioPlaying;
    public event EventHandler RadioPAused;

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

    public void Play(string radioUrl)
    {
      if (String.IsNullOrEmpty(radioUrl))
      {
        throw new ArgumentNullException("radioUrl", "You have to provide a network radio URL.");
      }

      media = new Media(lib, radioUrl, FromType.FromLocation);
      mediaPlayer.Media = media;
      mediaPlayer.Play();      
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
      RadioPAused.Invoke(this, null);
    }

    private void OnPlaying(object sender, EventArgs e)
    {
      Console.WriteLine("Play");
      RadioPlaying.Invoke(this, null);
    }
  }
}
