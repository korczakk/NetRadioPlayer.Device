using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace NetRadioPlayer.Device.IoTHub
{
  public class IoTDevice
  {
    private DeviceClient device;

    public DeviceClient GetDevice()
    {
      string cn = GetConnectionstring();
      if (String.IsNullOrEmpty(cn))
      {
        throw new Exception("Connection string is empty.");
      }

      device = DeviceClient.CreateFromConnectionString(cn, TransportType.Mqtt);
      Console.WriteLine("Device connected.");
      
      return device;
    }

    public async Task SendNotification(string notification)
    {
      var json = JsonConvert.SerializeObject(notification);
      var message = new Message(Encoding.ASCII.GetBytes(json));
      await this.device.SendEventAsync(message);
    }

    private string GetConnectionstring()
    {
      Console.WriteLine("Getting connection string to Iot Hub...");
      return ConfigurationManager.ConnectionStrings["IoT Hub"].ConnectionString;
    }
  }
}
