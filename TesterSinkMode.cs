using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VseriesControllerLibrary_V1;

namespace Scripts
{
    public class TesterSinkMode
    {
        public async void MultiRunScript(Dictionary<string, GRLDeviceList> deviceList, PortID portID = PortID.Port1, int delay = 500)
        {


            await Task.Run(() => Parallel.ForEach(deviceList, port =>
            {
                if (GrlVPdApiLib.Instance.Set_Controller_Mode(ControllerMode.Sink, port.Value.IPAddress))
                {
                    Console.WriteLine("Controller set to Sink mode SuccessFully", port.Value.IPAddress);

                }
                else
                {
                    Console.WriteLine("Controller set to Sink mode is failed ", port.Value.IPAddress);

                }
            }));
            await Task.Run(() => Parallel.ForEach(deviceList, port =>
            {
                var sourceCaps = GrlVPdApiLib.Instance.SourceCapabilities(portID, port.Value.IPAddress);
                if (sourceCaps.PDOlist.Count == 0)
                {
                    Console.WriteLine("Please connect the DUT");
                    return;
                }

                foreach (var pdo in sourceCaps.PDOlist)
                {

                    if (pdo.PdoType == PDOSupplyType.FixedSupply)
                    {

                        if ((PDOIndex)pdo.PDO_Index == PDOIndex.PDO2)
                        {
                            Console.WriteLine("----------------------------------------------------------------");
                            Console.WriteLine($"{DateTime.Now}: Controller- {port.Value.IPAddress} Requesting \nPDO Index-" + (PDOIndex)pdo.PDO_Index + "\nVoltage -" + pdo.Voltage + "\nCurrent - " + pdo.Current + "\nMax Operating Current - " + pdo.Current);
                            Console.WriteLine("----------------------------------------------------------------");

                            GrlVPdApiLib.Instance.RequestPDO(portID, (PDOIndex)pdo.PDO_Index, pdo.Current, pdo.Current, port.Value.IPAddress);

                            GrlVPdApiLib.Instance.SetVbusCurrent(pdo.Current, portID, VbusEload.On, VBUSModeConfig.CCMode, port.Value.IPAddress);
                            Thread.Sleep(1500);

                            var Voltagestatus = GrlVPdApiLib.Instance.GetVBUSData(portID, port.Value.IPAddress);
                            Console.WriteLine($"{DateTime.Now}: Controller- {port.Value.IPAddress} {Voltagestatus}");

                        }

                    }
                }



            }));



        }
    }
}
