using NetRadioPlayer.Device.IoTHub;
using System;
using System.Threading.Tasks;
using NetRadioPlayer.Device.Model;

namespace NetRadioPlayer.Device
{
  class Program
  {
    private static bool continuePlaying = true;
    private static IoTDevice iotDev;
    private static DeviceState deviceState = DeviceState.NotSet;

    static async Task Main(string[] args)
    {
      iotDev = new IoTDevice();
      iotDev.ConnectToIoTHub();

      var commandListener = new IotHubCommandListener(iotDev.Device);
      await commandListener.RegisterListener();
      commandListener.Shutdown += OnShuttingdown;
      commandListener.AskForState += OnAskForState;

      using (var radioPlayer = new RadioPlayer())
      {
        radioPlayer.RadioPaused += OnPaused;
        radioPlayer.RadioPlaying += OnPlaying;

        commandListener.Play += payload => radioPlayer.Play(payload.Uri);
        commandListener.Pause += x => radioPlayer.Pause();

        deviceState = DeviceState.DeviceReady;
        await iotDev.SendNotification("Radio player is ready", DeviceState.DeviceReady, String.Empty);

        while (continuePlaying)
        {
        }
      }

      deviceState = DeviceState.TurnedOff;
      Console.WriteLine("Finish.");
    }

    private static async void OnAskForState(CommandPayload commandPayload)
    {
      await iotDev.SendNotification("Current status...", deviceState, String.Empty);
    }

    private static async void OnShuttingdown(CommandPayload commandPayload)
    {
      await iotDev.SendNotification("Shutting down...", DeviceState.TurnedOff, String.Empty);
      continuePlaying = false;      
      SystemShutdown.Shutdown();
    }

    private static async void OnPlaying(string uri)
    {
      deviceState = DeviceState.Playing;
      await iotDev.SendNotification("Playing", DeviceState.Playing, uri);
    }

    private static async void OnPaused(string uri)
    {
      deviceState = DeviceState.Paused;
      await iotDev.SendNotification("Paused", DeviceState.Paused, String.Empty);      
    }
  }
}
