using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BangOnline.Common
{
    [Serializable]
    public class DataToSend
    {
        public string ipAddr;
        public Command command;
        public object data;

        public static int bufferSize = 2048;

        public DataToSend(string ip, Command cmd, object obj)
        {
            ipAddr = ip;
            command = cmd;
            data = obj;
        }

        public static string Help()
        {
            string help = string.Empty;
            help += @"\run - Débute la partie" + "\n";
            help += @"\card x - Jouer la carte x de votre main" + "\n";
            help += @"\end - Termine le tour";
            return help;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static void SendData(string ip, Command cmd, object obj, Stream stream)
        {
            DataToSend dataToSend = new DataToSend(ip, cmd, obj);
            byte[] byteToSend = ISerialize.Serialize(dataToSend);
            stream.Write(byteToSend, 0, byteToSend.Length);
        }
    }
}
