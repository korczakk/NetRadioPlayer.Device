using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace NetRadioPlayer.Device.IoTHub
{
  public class IotHubCommandListener
  {    
    private string methodResponse = JsonConvert.SerializeObject("Recieved and processing");
    private IoTDevice device;

    public IotHubCommandListener(IoTDevice device)
    {      
      this.device = device;
    }

    public async Task<bool> RegisterListener()
    {
      try
      {
        await device.ClientDevice.SetMethodHandlerAsync("play", OnStartPlaying, null);
        await device.ClientDevice.SetMethodHandlerAsync("pause", OnPausePlayer, null);
        await device.ClientDevice.SetMethodHandlerAsync("shutdown", OnShutDown, null);
        await device.ClientDevice.SetMethodHandlerAsync("askforstate", OnAskForStatus, null);
        await device.ClientDevice.SetMethodHandlerAsync("setvolume", OnSetVolume, null);

        return true;
      }
      catch
      {
        return false;
      }
    }

    private async Task<MethodResponse> OnSetVolume(MethodRequest request, object userContext)
    {
      CommandPayload payload = JsonConvert.DeserializeObject<CommandPayload>(request.DataAsJson);

      await device.SetVolume(payload.VolumePercent);

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnStartPlaying(MethodRequest request, object userContext)
    {
      CommandPayload payload = JsonConvert.DeserializeObject<CommandPayload>(request.DataAsJson);

      await device.Play(payload.Uri);

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnPausePlayer(MethodRequest request, object userContext)
    {
      await device.Pause();

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnShutDown(MethodRequest request, object userContext)
    {
      await device.ShutDown();

      string status = JsonConvert.SerializeObject("Shutting down...");
      return new MethodResponse(Encoding.ASCII.GetBytes(status), 200);
    }

    private async Task<MethodResponse> OnAskForStatus(MethodRequest request, object userContext)
    {
      await device.GiveCurrentState();

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }
  }
}
