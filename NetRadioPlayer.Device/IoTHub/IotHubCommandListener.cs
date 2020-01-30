using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NetRadioPlayer.Device.IoTHub
{
  public class IotHubCommandListener
  {    
    private string methodResponse = JsonConvert.SerializeObject("Recieved and processing");
    private DeviceClient device;

    public IotHubCommandListener(DeviceClient device)
    {      
      this.device = device;
    }

    public event CommandHandler Play;
    public event CommandHandler Pause;
    public event CommandHandler Shutdown;
    public event CommandHandler AskForState;
    public event CommandHandler SetVolume;

    public delegate void CommandHandler(CommandPayload commandPayload);

    public async Task RegisterListener()
    {
      await device.SetMethodHandlerAsync("play", OnStartPlaying, null);
      await device.SetMethodHandlerAsync("pause", OnPausePlayer, null);
      await device.SetMethodHandlerAsync("shutdown", OnShutDown, null);
      await device.SetMethodHandlerAsync("askforstate", OnAskForStatus, null);
      await device.SetMethodHandlerAsync("setvolume", OnSetVolume, null);
    }

    private async Task<MethodResponse> OnSetVolume(MethodRequest request, object userContext)
    {
      CommandPayload payload = JsonConvert.DeserializeObject<CommandPayload>(request.DataAsJson);

      SetVolume.Invoke(payload);

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnStartPlaying(MethodRequest request, object userContext)
    {
      CommandPayload payload = JsonConvert.DeserializeObject<CommandPayload>(request.DataAsJson);

      Play.Invoke(payload);

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnPausePlayer(MethodRequest request, object userContext)
    {
      Pause.Invoke(null);

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnShutDown(MethodRequest request, object userContext)
    {
      Shutdown.Invoke(null);

      string status = JsonConvert.SerializeObject("Shutting down...");
      return new MethodResponse(Encoding.ASCII.GetBytes(status), 200);
    }

    private async Task<MethodResponse> OnAskForStatus(MethodRequest request, object userContext)
    {
      AskForState.Invoke(null);

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }
  }
}
