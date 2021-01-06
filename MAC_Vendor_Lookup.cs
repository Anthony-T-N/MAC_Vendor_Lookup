using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace MAC_Vendor_Lookup
{
    class Program
    {
        static void Main(string[] args)
        {
            Program main_program = new Program();
            string mac_addresses = main_program.Get_Mac_Address();
            Console.WriteLine(mac_addresses);
            Console.WriteLine(Vendor_Lookup(mac_addresses).Result);
        }
        private string Get_Mac_Address()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string mac_address = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Debug.WriteLine(
                    "Found MAC Address: " + nic.GetPhysicalAddress() +
                    " Type: " + nic.NetworkInterfaceType);

                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    Debug.WriteLine("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    mac_address = tempMac;
                }
            }
            return mac_address;
        }
        private static async Task<string> Vendor_Lookup(string mac_address)
        {
            /*
            WebRequest request = WebRequest.Create("http://api.macvendors.com/" + mac_address);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string html = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }
            return html;
            */
            HttpClient client = new HttpClient();
            var uri = await client.GetStringAsync("http://api.macvendors.com/" + mac_address);
            return uri;
        }
    }
}
