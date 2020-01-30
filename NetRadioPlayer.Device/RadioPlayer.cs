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
    }

    public string CurrentlyPlaying { get; private set; }
    public int CurrentVolume { get; private set; }

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
      CurrentVolume = mediaPlayer.Volume;
    }

    public void Pause()
    {
      mediaPlayer.Pause();
    }

    public void SetVolume(int volumePercent)
    {
      mediaPlayer.Volume = volumePercent;

      CurrentVolume = volumePercent;
    }

    public void Dispose()
    {
      lib.Dispose();
      
      if(media != null)
        media.Dispose();

      mediaPlayer.Dispose();      
      Console.WriteLine("Objects disposed");
    }
  }
}
