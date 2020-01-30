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
      Console.WriteLine("Check if netowrk connection is available.");
      while (!ConnectivityChecker.CheckIfDeviceConected())
      {
        Console.WriteLine("No network connection. Trying in 3 seconds.");
        await Task.Delay(TimeSpan.FromSeconds(3));
      }

      iotDev = new IoTDevice();
      iotDev.ConnectToIoTHub();

      var commandListener = new IotHubCommandListener(iotDev.Device);
      await commandListener.RegisterListener();
      commandListener.Shutdown += OnShuttingdown;

      using (var radioPlayer = new RadioPlayer())
      {
        radioPlayer.RadioPaused += OnPaused;
        radioPlayer.RadioPlaying += OnPlaying;
        radioPlayer.VolumeChanged += OnVolumeChanged;

        commandListener.Play += payload => radioPlayer.Play(payload.Uri);
        commandListener.Pause += x => radioPlayer.Pause();
        commandListener.AskForState += async cmd => await iotDev.SendNotification("Current status...", deviceState, new MediaPlayerState(radioPlayer.CurrentlyPlaying, radioPlayer.CurrentVolume));

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

    private static async void OnVolumeChanged(string uri, int volumePercent)
    {
      await iotDev.SendNotification("VolumeChanged", DeviceState.Playing, new MediaPlayerState(uri, volumePercent));
    }

    private static async void OnShuttingdown(CommandPayload commandPayload)
    {
      await iotDev.SendNotification("Shutting down...", DeviceState.TurnedOff, new MediaPlayerState(String.Empty, 0));
      continuePlaying = false;      
    }

    private static async void OnPlaying(string uri, int volumePercent)
    {
      deviceState = DeviceState.Playing;
      await iotDev.SendNotification("Playing", DeviceState.Playing, new MediaPlayerState(uri, volumePercent));
    }

    private static async void OnPaused(string uri, int volumePercent)
    {
      deviceState = DeviceState.Paused;
      await iotDev.SendNotification("Paused", DeviceState.Paused, new MediaPlayerState(String.Empty, volumePercent));      
    }
  }
}
