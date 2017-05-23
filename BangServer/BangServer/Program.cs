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

        static Random rand;

        static void Main(string[] args)
        {
            rand = new Random();

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
            while (true)
            {
                TcpClient TcpClient = server.AcceptTcpClient();

                Client client = new Client(TcpClient);
                bool isPresent = false;

                foreach (Client c in clients)
                {
                    if (c.ipAddr == client.ipAddr)
                    {
                        isPresent = true;
                        break;
                    }
                }
                if (!isPresent)
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
                    if (!listenMJ.IsAlive)
                    {
                        listenMJ.Start();
                    }
                }
                if (clients.Count == 7)
                {
                    InitializeParty();
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
                    InitializeParty();
                }
            }
        }
        #endregion

        #region Init Party
        static Deck<Card> InitializeCard() // A REVOIR
        {
            List<CardRepartition> cardRepartition = ISerialize.Deserialize<List<CardRepartition>>(@"_Datas/Cards.json");

            Deck<Card> cards = new Deck<Card>();

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

        static void AssignRole(int numberOfPlayer, int sherif, int adjudant, int renegat, int horsLaLoi)
        {
            while (sherif != 0 || adjudant != 0 || renegat != 0 || horsLaLoi != 0)
            {
                int index = rand.Next(0, numberOfPlayer);
                Client client = clients[index];
                if (client.role == Role.None)
                {
                    if (sherif != 0)
                    {
                        client.role = Role.Sherif;
                        client.character.maxLife++;
                        sherif--;
                    }
                    else if (adjudant != 0)
                    {
                        client.role = Role.Adjudant;
                        adjudant--;
                    }
                    else if (renegat != 0)
                    {
                        client.role = Role.Renegat;
                        renegat--;
                    }
                    else if (horsLaLoi != 0)
                    {
                        client.role = Role.HorsLaLoi;
                        horsLaLoi--;
                    }
                }
            }
        }

        static void InitializeParty()
        {
            #region Initialization Card
            Deck<Card> cards = InitializeCard();
            #endregion

            #region Assign Class
            Deck<Character> characters = ISerialize.Deserialize<Deck<Character>>(@"_Datas\Characters.json");
            foreach(Client client in clients)
            {
                client.character = characters.GetRandomElement(rand, true);
            }
            #endregion

            #region Assign Role
            int numberPlayer = clients.Count;
            if (numberPlayer == 4)
            {
                AssignRole(numberPlayer, 1, 0, 1, 2);
            }
            else if (numberPlayer == 5)
            {
                AssignRole(numberPlayer, 1, 1, 1, 2);
            }
            else if (numberPlayer == 6)
            {
                AssignRole(numberPlayer, 1, 1, 1, 3);
            }
            else if (numberPlayer == 7)
            {
                AssignRole(numberPlayer, 1, 2, 1, 3);
            }
            #endregion

            #region Assign Card
            foreach (Client client in clients)
            {
                client.SetCards(cards.PopFirstElement(client.character.maxLife));
            }
            #endregion
        }
        #endregion

    }
}
