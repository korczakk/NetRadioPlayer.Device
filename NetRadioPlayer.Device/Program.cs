using Microsoft.Azure.Devices.Client;
using NetRadioPlayer.Device.IoTHub;
using System;
using System.Threading.Tasks;

namespace NetRadioPlayer.Device
{
  class Program
  {
    private static bool continuePlaying = true;

    static async Task Main(string[] args)
    {
      SystemShutdown.ShuttingDown += OnShuttingDown;

      var iotDev = new IoTDevice();
      DeviceClient device = iotDev.GetDevice();
            
      using (var radioPlayer = new RadioPlayer())
      {
        radioPlayer.RadioPAused += async (object sender, EventArgs e) => { await iotDev.SendNotification("Paused"); };
        radioPlayer.RadioPlaying += async (object sender, EventArgs e) => { await iotDev.SendNotification("Playing"); }; ;

        var commandListener = new IotHubCommandListener(radioPlayer);
        await commandListener.RegisterListener(device);

        await iotDev.SendNotification("Radio player is ready");

        while (continuePlaying)
        {
        }
      }
    }

    public static void OnShuttingDown(object sender, EventArgs e)
    {
      continuePlaying = false;
    }

  }
}
