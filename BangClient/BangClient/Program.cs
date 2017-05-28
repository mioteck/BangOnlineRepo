using BangOnline.Common;
using System;
using System.Net.Sockets;
using System.Threading;

namespace BangClient
{
    class Program
    {
        static TcpClient client;
        static NetworkStream stream;

        static string myIpAdress;

        static Thread waitingData;
        static Thread sendData;

        static void Main(string[] args)
        {
            /* Console.Write("Adresse Ip du serveur : ");
             string ipToConnect = Console.ReadLine();
             Console.Write("Port du serveur : ");
             int portToConnect = int.Parse(Console.ReadLine());

             client = new TcpClient(ipToConnect, portToConnect);*/
            client = new TcpClient("192.168.1.68", 1337);
            stream = client.GetStream();


            myIpAdress = DataToSend.GetLocalIPAddress();

            waitingData = new Thread(WaitingData);
            waitingData.Start();

            sendData = new Thread(SendData);
            sendData.Start();
        }

        #region Stream
        static void SendData()
        {
            while(true)
            {
                string command = Console.ReadLine();
                if (string.IsNullOrEmpty(command)) continue;
                DispatcherSend(command);
            }
        }

        static void CloseStream()
        {
            sendData.Abort();
            waitingData.Abort();
            stream.Close();
            client.Close();
        }

        #endregion

        static void WaitingData()
        {
            while(true)
            {
                byte[] bytes = new byte[DataToSend.bufferSize];
                int result = stream.Read(bytes, 0, bytes.Length);
                if (result == -1) continue;
                DataToSend data = (DataToSend)ISerialize.Deserialize(bytes);
                DispatcherReceive(data);
            }
        }

        #region Dispatcher
        static void DispatcherReceive(DataToSend data)
        {
            if(data.command == Command.NbPlayer)
            {
                int nbPlayer = (int)data.data;
                Console.WriteLine("Il y a " + nbPlayer + " joueurs dans la partie");
            }
            else if(data.command == Command.StringToDraw)
            {
                Console.WriteLine((string)data.data);
            }
            else if(data.command == Command.Quit)
            {
                Console.WriteLine((string)data.data);
                CloseStream();
            }
            
        }

        static void DispatcherSend(string command)
        {
            if(command == @"\nbPlayer")
            {
                DataToSend.SendData(myIpAdress, Command.NbPlayer, null, stream);
            }
            else if(command == @"\GetCards")
            {
                DataToSend.SendData(myIpAdress, Command.GetCards, null, stream);
            }
            else if(command == @"\Quit")
            {
                DataToSend.SendData(myIpAdress, Command.Quit, null, stream);
            }
            else
            {
                Console.WriteLine("La commande n'existe pas !");
            }
        }
        #endregion
    }
}
