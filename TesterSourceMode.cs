using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VseriesControllerLibrary_V1;

namespace Scripts
{
    public class TesterSourceMode
    {
        public async void RunScript(Dictionary<string, GRLDeviceList> deviceList, PortID portID = PortID.Port1, int delay = 500)
        {
            await Task.Run(() => Parallel.ForEach(deviceList, port =>
            {
                // This Function will Set Controller MOde as Source

                if (GrlVPdApiLib.Instance.Set_Controller_Mode(ControllerMode.Source, port.Value.IPAddress))
                {
                    Console.WriteLine($"{DateTime.Now}:{ port.Value.IPAddress}:Controller set to source mode");

                }
                else
                {
                    Console.WriteLine($"{DateTime.Now}:{ port.Value.IPAddress}:Controller set to source mode is failed ");

                }
            }));

            await Task.Run(() => Parallel.ForEach(deviceList, port =>
            {

                ConfigureSourceCapability obj = new ConfigureSourceCapability();
                obj.AddFixedSupply((uint)(5000), (uint)(3000));
                obj.AddFixedSupply((uint)(9000), (uint)(3000));

                //This Function will SetSourcecapabliiltiy
                if (GrlVPdApiLib.Instance.SetSourceCapability(PortID.Port1, obj, port.Value.IPAddress))
                {

                    Console.WriteLine($"{ port.Value.IPAddress} :Source capabilities configured");
                }
                else
                {

                    Console.WriteLine($"{ port.Value.IPAddress}:Source capabilities configuration failed");
                }
                Thread.Sleep(1500);

                //This Function will Get Vbus Data
                var Voltagestatus = GrlVPdApiLib.Instance.GetVBUSData(portID, port.Value.IPAddress);
                Console.WriteLine($"{DateTime.Now}: Controller- {port.Value.IPAddress} {Voltagestatus}");

            }));
        }
    }
}
