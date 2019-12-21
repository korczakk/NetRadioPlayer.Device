using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace NetRadioPlayer.Device.IoTHub
{
  public class IotHubCommandListener
  {
    private readonly RadioPlayer radioPlayer;
    private string methodResponse = JsonConvert.SerializeObject("Recieved and processing");
    
    public IotHubCommandListener(RadioPlayer radioPlayer)
    {
      this.radioPlayer = radioPlayer;
    }

    public async Task RegisterListener(DeviceClient device)
    {
      await device.SetMethodHandlerAsync("play", OnStartPlaying, null);
      await device.SetMethodHandlerAsync("pause", OnPausePlayer, null);
      await device.SetMethodHandlerAsync("shutdown", OnShutDown, null);
    }

    private async Task<MethodResponse> OnStartPlaying(MethodRequest request, object userContext)
    {
      CommandPayload payload = JsonConvert.DeserializeObject<CommandPayload>(request.DataAsJson);

      radioPlayer.Play(payload.Uri);

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnPausePlayer(MethodRequest request, object userContext)
    {
      radioPlayer.Pause();

      return new MethodResponse(Encoding.ASCII.GetBytes(methodResponse), 200);
    }

    private async Task<MethodResponse> OnShutDown(MethodRequest request, object userContext)
    {
      radioPlayer.Pause();
      SystemShutdown.Shutdown();

      string status = JsonConvert.SerializeObject("Shutting down...");
      return new MethodResponse(Encoding.ASCII.GetBytes(status), 200);
    }
  }
}
