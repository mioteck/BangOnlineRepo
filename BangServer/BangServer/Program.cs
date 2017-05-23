using BangOnline.Cards;
using BangOnline.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BangServer
{
    class Program
    {
        static List<Client> clients;
        static TcpListener server;

        static Thread waitingSaloon;
        static Thread listenMJ;

        static void Main(string[] args)
        {
            clients = new List<Client>();

            server = new TcpListener(IPAddress.Any, 1337);

            waitingSaloon = new Thread(WaitingSaloon);
            listenMJ = new Thread(ListenMJ);

            //waitingSaloon.Start();

            InitializeCard();
        }

        #region SendMessage
        static void SendMessage(int ID, string message)
        {
            if (clients.Count < ID)
            {
                Console.WriteLine("!!! Il n'y a aucun joueur de connecté !!!");
                return;
            }
            Client client = clients[ID];

            client.SendMessage(message);
        }

        static void SendMessage(int ID, object obj)
        {
            if (clients.Count < ID)
            {
                Console.WriteLine("!!! Il n'y a aucun joueur de connecté !!!");
                return;
            }
            Client client = clients[ID];

            client.SendMessage(obj);
        }

        static void SendMessageToAll(string message)
        {
            if (clients.Count == 0)
            {
                Console.WriteLine("!!! Il n'y a aucun joueur de connecté !!!");
                return;
            }
            foreach (Client client in clients)
            {
                client.SendMessage(message);
            }
        }

        static void SendMessageToAll(object obj)
        {
            if (clients.Count == 0)
            {
                Console.WriteLine("!!! Il n'y a aucun joueur de connecté !!!");
                return;
            }
            foreach (Client client in clients)
            {
                client.SendMessage(obj);
            }
        }
        #endregion

        #region Saloon
        static void WaitingSaloon()
        {
            while(true)
            {
                TcpClient TcpClient = server.AcceptTcpClient();

                Client client = new Client(TcpClient);
                bool isPresent = false;

                foreach(Client c in clients)
                {
                    if(c.ipAddr == client.ipAddr)
                    {
                        isPresent = true;
                        break;
                    }
                }
                if(!isPresent)
                {
                    Console.WriteLine("Un joueur vient de se connecter : " + client.ipAddr);
                    client.ID = clients.Count;
                    clients.Add(client);
                }
                else
                {
                    client.SendMessage("Vous êtes déjà connecté au serveur !");
                }

                if (clients.Count >= 2)
                {
                    SendMessage(0, "Il y a 2 joueurs, voulez vous commencez la partie? (\run pour lancer la partie)");
                    if(!listenMJ.IsAlive)
                    {
                        listenMJ.Start();
                    }
                }
            }
        }

        static void ListenMJ()
        {
            if (clients.Count == 0)
            {
                Console.WriteLine("!!! Il n'y a aucun joueur de connecté !!!");
                return;
            }
            Client client = clients[0];
            while (true)
            {
                byte[] bytes = client.ReceiveMessage(64); // Peut être source de problème à check
                if (bytes.Length == 0) continue;
                string message = ISerialize.DeserializeString(bytes);
                if (message == "\run")
                {
                    waitingSaloon.Join();
                    listenMJ.Join();
                    // LANCEMENT DE LA PARTIE : C'EST PARTIE BB
                }
            }
        }
        #endregion

        static Deck<Card> InitializeCard()
        {
            Deck<Bang> bangs = ISerialize.Deserialize<Deck<Bang>>(@"C:\Users\Bousq\Documents\Visual Studio 2015\Projects\BangOnlineRepo\BangServer\BangServer\bin\Debug\_Datas\Bang.json");


            return new Deck<Card>();
        }

        static void Initialize()
        {

        }
    }
}
