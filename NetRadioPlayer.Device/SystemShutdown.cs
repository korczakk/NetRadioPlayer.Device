using System;
using System.Diagnostics;

namespace NetRadioPlayer.Device
{
  public class SystemShutdown
  {
    public static event EventHandler ShuttingDown;

    public static void Shutdown()
    {
      ShuttingDown.Invoke(null, null);
      Process.Start("/sbin/shutdown", "-h now");
    }
  }
}
