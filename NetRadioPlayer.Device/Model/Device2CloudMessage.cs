using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetRadioPlayer.Device.Model
{
  public class Device2CloudMessage
  {
    public string MessageContent { get; private set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public DeviceState DeviceState { get; private set; }

    public string JsonPayload { get; private set; }

    public Device2CloudMessage(string message, DeviceState state, string jsonPayload)
    {
      MessageContent = message;
      DeviceState = state;
      JsonPayload = jsonPayload;
    }
  }
}
