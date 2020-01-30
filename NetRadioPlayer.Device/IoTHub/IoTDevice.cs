using Microsoft.Azure.Devices.Client;
using NetRadioPlayer.Device.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace NetRadioPlayer.Device.IoTHub
{
  public class IoTDevice
  {
    private readonly RadioPlayer radioPlayer;

    public DeviceClient ClientDevice { get; private set; }

    public IoTDevice(RadioPlayer radioPlayer)
    {
      this.radioPlayer = radioPlayer;
    }

    public void ConnectToIoTHub()
    {
      string cn = GetConnectionstring();
      if (String.IsNullOrEmpty(cn))
      {
        throw new Exception("Connection string is empty.");
      }

      ClientDevice = DeviceClient.CreateFromConnectionString(cn, TransportType.Mqtt);
      Console.WriteLine("Device connected.");
    }

    public async Task Play(string uri)
    {
      radioPlayer.Play(uri);

      Program.deviceState = DeviceState.Playing;
      await SendNotification("Playing", DeviceState.Playing, new MediaPlayerState(uri, radioPlayer.CurrentVolume));
    }

    public async Task Pause()
    {
      radioPlayer.Pause();

      Program.deviceState = DeviceState.Paused;
      await SendNotification("Paused", DeviceState.Paused, new MediaPlayerState(String.Empty, radioPlayer.CurrentVolume));
    }

    public async Task SetVolume(int volumePercent)
    {
      radioPlayer.SetVolume(volumePercent);

      await SendNotification("Volume has been changed",
        Program.deviceState,
        new MediaPlayerState(radioPlayer.CurrentlyPlaying, radioPlayer.CurrentVolume));
    }

    public async Task GiveCurrentState()
    {
      await SendNotification("Current status...", Program.deviceState, new MediaPlayerState(radioPlayer.CurrentlyPlaying, radioPlayer.CurrentVolume));
    }

    public async Task ShutDown()
    {
      await SendNotification("Shutting down...", DeviceState.TurnedOff, new MediaPlayerState(String.Empty, 0));
      Program.deviceState = DeviceState.TurnedOff;
      Program.continuePlaying = false;
    }

    public async Task SendNotification(string notificationMessage, DeviceState state, MediaPlayerState playerState)
    {
      var messagePayload = new Device2CloudMessage(notificationMessage, state, playerState);
      var json = JsonConvert.SerializeObject(messagePayload);
      var message = new Message(Encoding.ASCII.GetBytes(json));
      await ClientDevice.SendEventAsync(message);
    }

    private string GetConnectionstring()
    {
      Console.WriteLine("Getting connection string to Iot Hub...");
      return ConfigurationManager.ConnectionStrings["IoT Hub"].ConnectionString;
    }
  }
}
