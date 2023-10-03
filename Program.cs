using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VseriesControllerLibrary_V1;
namespace Scripts
{
    public class Program
    {
        #region Public Module 
        public static void Help()
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Please select the script");
            Console.WriteLine("1.Tester as Sink Mode");
            Console.WriteLine("2.Tester as Source Mode");
            Console.WriteLine("Type \"Help\" for commands");
            Console.WriteLine("Type 1, 2 ...  and press Enter ");
            Console.WriteLine("----------------------------------------------");
        }
        #endregion
        static void Main()
        {
            // This is called if the app type is V-PWR
            V_PWR_Script();
        }
        #region Private Module 

        /// <summary>
        /// This function contains V-PWR sample scripts 
        /// </summary>
        private  static void V_PWR_Script()
        {

            List<string> ipAddressList = new List<string>();
            string exit = "Y";
            do
            {
                Console.WriteLine($"Enter the IP Address :");
                string ipaddress = Console.ReadLine();
                ipAddressList.Add(ipaddress);
                Console.WriteLine($"Type Y (Yes) if you want to add another controller else N (NO) to exit:");
                exit = Console.ReadLine().ToUpper();

            } while (exit != "N" && exit != "NO");

            foreach (var ipAddress in ipAddressList)
            {
                if (GrlVPdApiLib.Instance.InitilizeController(ipAddress))
                {
                    Console.WriteLine($"Connection establised successful : {ipAddress}");
                }
                else
                {
                    Console.WriteLine($"Connection establised Failed : {ipAddress}");
                }
            }


            var connectedDeviceList = GrlVPdApiLib.Instance.GetGRLDeviceList;


            Help();

            Console.WriteLine("Please Enter the Commands:");
            while (true)
            {
                var read = Console.ReadLine().ToUpper();
                Console.WriteLine("----------------------------------------------");

                if (read == "HELP")
                {
                    Help();
                }
                else if (read == "EXIT")
                {
                    break;
                }
                else if (read == "1")
                {
                    try
                    {
                        TesterSinkMode testerSinkMode = new TesterSinkMode();
                        testerSinkMode.MultiRunScript(connectedDeviceList);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    Console.ReadLine();

                }
                else if (read == "2")
                {
                    TesterSourceMode testerSource = new TesterSourceMode();
                    testerSource.RunScript(connectedDeviceList);
                }
                else
                {
                    Console.WriteLine("Wrong input, Please try again");
                }
            }
        }
    }
}
#endregion
