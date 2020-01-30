namespace NetRadioPlayer.Device.Model
{
  public class MediaPlayerState
  {
    public int VolumePercent { get; private set; }

    public string RadioUrl { get; private set; }

    public MediaPlayerState(string radioUrl, int volumePercent)
    {
      RadioUrl = radioUrl;
      VolumePercent = volumePercent;
    }
  }
}
