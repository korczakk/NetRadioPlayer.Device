using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetRadioPlayer.Device.Model
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum DeviceState
  {
    DeviceReady,
    Paused,
    Playing,
    NotSet,
    TurnedOff
  }
}