using System;
using System.Diagnostics;

namespace NetRadioPlayer.Device
{
  public class SystemShutdown
  {
    public static void Shutdown()
    {
      Console.WriteLine("Shutting down...");
      Process.Start("sudo", "poweroff");
    }
  }
}
