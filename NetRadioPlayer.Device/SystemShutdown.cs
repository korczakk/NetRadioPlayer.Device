using System;
using System.Diagnostics;

namespace NetRadioPlayer.Device
{
  public class SystemShutdown
  {
    public static void Shutdown()
    {
      Process.Start("sudo", "/sbin/shutdown -h now");
    }
  }
}
