using System.Net;

namespace NetRadioPlayer.Device
{
  public class ConnectivityChecker
  {
    public static bool CheckIfNetworkConnected()
    {
      try
      {
        using (var client = new WebClient())
        using (var request = client.OpenRead("http://google.com"))
        {
          return true;
        }
      }
      catch
      {
        return false;
      }
    }
  }
}
