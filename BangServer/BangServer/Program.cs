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

            waitingSaloon.Start();
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

        static Deck<Card> InitializeCard() // A REVOIR
        {
            List<CardRepartition> cardRepartition = ISerialize.Deserialize<List<CardRepartition>>(@"_Datas/Cards.json");

            Deck<Card> cards = new Deck<Card>();

            Random rand = new Random();
            foreach (CardRepartition CR in cardRepartition)
            {
                for (int i = 0; i < CR.number; i++)
                {
                    
                    switch (CR.type)
                    {
                        case CardType.Bang:
                            Bang b = new Bang("Bang", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.APorteeDeTire, "Enlève un point de vie à un joueur à porté de tir", -1);
                            cards.Add(b);
                            break;

                        case CardType.Discard:
                            Discard d = new Discard("Discard", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.SoiMeme, "Fait échouer un Bang", -1);
                            cards.Add(d);
                            break;

                        case CardType.Draw:
                            Draw dd = new Draw("Draw", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.SoiMeme, "Pioche 2 cartes", -1);
                            cards.Add(dd);
                            break;

                        case CardType.Duel:
                            Duel ddd = new Duel("Duel", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.NimporteQui, "Provoque un duel", -1);
                            cards.Add(ddd);
                            break;

                        case CardType.Heal:
                            Heal h = new Heal("Heal", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.NimporteQui, "Soigne un point de vie", -1);
                            cards.Add(h);
                            break;

                        case CardType.Miss:
                            Miss m = new Miss("Miss", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.SoiMeme, "Esquive un Bang", -1);
                            cards.Add(m);
                            break;

                        case CardType.Jail:
                            Jail j = new Jail("Jail", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.NimporteQui, "Emprisonne quelqu'un");
                            cards.Add(j);
                            break;

                        case CardType.ModRange:
                            ModRange mr = new ModRange("Mustang", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.SoiMeme, "Distance +1 pour les autres joueurs");
                            cards.Add(mr);
                            break;

                        case CardType.Stash:
                            Stash s = new Stash("Stash", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.SoiMeme, "Quand Bang reçu, piocher une carte, si coeur alors esquive");
                            cards.Add(s);
                            break;

                        case CardType.Weapon:
                            Weapon w = new Weapon("Pistoulet", (Couleur)rand.Next(0, 4), (Value)rand.Next(0, 13), Cible.SoiMeme, "Augmente la porté de 1");
                            cards.Add(w);
                            break;

                        default:
                            break;
                    }
                }

            }

            cards.Shuffle();

            return cards;
        }

    }
}
