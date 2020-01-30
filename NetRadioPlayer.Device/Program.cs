using NetRadioPlayer.Device.IoTHub;
using System;
using System.Threading.Tasks;
using NetRadioPlayer.Device.Model;

namespace NetRadioPlayer.Device
{
  class Program
  {
    public static DeviceState deviceState = DeviceState.NotSet;
    public static bool continuePlaying = true;

    static async Task Main(string[] args)
    {
      Console.WriteLine("Check if netowrk connection is available.");
      while (!ConnectivityChecker.CheckIfNetworkConnected())
      {
        Console.WriteLine("No network connection. Trying in 3 seconds.");
        await Task.Delay(TimeSpan.FromSeconds(3));
      }    

      using (var radioPlayer = new RadioPlayer())
      {        
        var iotDev = new IoTDevice(radioPlayer);
        iotDev.ConnectToIoTHub();
        
        var commandListener = new IotHubCommandListener(iotDev);
        int attempts = 0;
        do
        {
          bool registartionStatus = await commandListener.RegisterListener();

          attempts = registartionStatus ? 3 : attempts = +1;

        } while (attempts < 3);

        deviceState = DeviceState.DeviceReady;
        await iotDev.SendNotification("Radio player is ready", DeviceState.DeviceReady, new MediaPlayerState(String.Empty, 0));

        while (continuePlaying)
        {
        }
      }

      deviceState = DeviceState.TurnedOff;
      Console.WriteLine("Finish.");
      
      SystemShutdown.Shutdown();
    }
  }
}
