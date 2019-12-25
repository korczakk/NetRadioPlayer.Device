namespace NetRadioPlayer.Device.Model
{
  public class Device2CloudMessage
  {
    public string MessageContent { get; private set; }

    public DeviceState DeviceState { get; private set; }

    public Device2CloudMessage(string message, DeviceState state)
    {
      MessageContent = message;
      DeviceState = state;
    }
  }
}
