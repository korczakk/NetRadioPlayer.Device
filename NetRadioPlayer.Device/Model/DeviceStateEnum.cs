using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace NetRadioPlayer.Device.Model
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum DeviceState
  {
    [EnumMember(Value = "0")]
    DeviceReady,
    [EnumMember(Value = "1")]
    Paused,
    [EnumMember(Value = "2")]
    Playing,
    [EnumMember(Value = "3")]
    NotSet,
    [EnumMember(Value = "4")]
    TurnedOff
  }
}