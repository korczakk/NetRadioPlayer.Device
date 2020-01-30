using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace NetRadioPlayer.Device.Model
{
  public class Device2CloudMessage
  {
    public string MessageContent { get; private set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public DeviceState DeviceState { get; private set; }

    public MediaPlayerState PlayerState { get; private set; }

    public DateTime EventTime { get; set; }
    public Device2CloudMessage(string message, DeviceState state, MediaPlayerState playerState)
    {
      MessageContent = message;
      DeviceState = state;
      PlayerState = playerState;
      EventTime = DateTime.UtcNow;
    }
  }
}
