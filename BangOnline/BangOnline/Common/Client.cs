using BangOnline.Cards;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace BangOnline.Common
{
    /// <summary>
    /// The Client class is all the information concerning the player
    /// </summary>
    public class Client : IArrayString
    {
        /// <summary>
        /// The ID of the player
        /// </summary>
        public int ID;
        /// <summary>
        /// the network info of the client
        /// </summary>
        public TcpClient tcpClient;
        /// <summary>
        /// The stream of the network info
        /// </summary>
        public NetworkStream stream;
        /// <summary>
        /// The IP address of the client
        /// </summary>
        public string ipAddr;

        /// <summary>
        /// The role of the player
        /// </summary>
        public Role role;
        /// <summary>
        /// the incarnated character
        /// </summary>
        public Character character;
        /// <summary>
        /// The list of cards owned by the player
        /// </summary>
        public Deck<Card> cards;
        /// <summary>
        /// The list of the equipment equiped
        /// </summary>
        public Deck<Equipment> equipments;
        /// <summary>
        /// The shooting range
        /// </summary>
        public int portee;
        /// <summary>
        /// Show if the player is alive or not
        /// </summary>
        public bool isAlive;

        public bool isRoleVisible;

        static int id = 0;

        public Client(TcpClient c)
        {
            tcpClient = c;
            stream = tcpClient.GetStream();

            IPEndPoint ipClient = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            ipAddr = ipClient.Address.ToString();

            role = Role.None;

            cards = new Deck<Card>();
            equipments = new Deck<Equipment>();

            isAlive = true;
            isRoleVisible = false;

            ID = id;
            id++;

            portee = 1;
        }

        public Client() // For test
        {
            role = Role.None;

            cards = new Deck<Card>();
            equipments = new Deck<Equipment>();

            ID = id;
            id++;

            isAlive = true;

            portee = 1;
        }

        #region GamePlay
        public void SetCards(Deck<Card> c)
        {
            cards.AddRange(c);
        }

        public void SetCard(Card c)
        {
            cards.Add(c);
        }

        public string ShowCards()
        {
            string result = string.Empty;
            foreach(Card c in cards)
            {
                result += c.ToString() + "\n";
            }
            return result;
        }
        #endregion

        #region Stream
        public void SendMessage(string message)
        {
            byte[] bytesToSend = ISerialize.Serialize(message);

            stream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        public void SendMessage(object obj)
        {
            byte[] bytesToSend = ISerialize.Serialize(obj);

            stream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        public byte[] ReceiveMessage(int bufferSize)
        {
            byte[] bytes = new byte[bufferSize];
            int result = stream.Read(bytes, 0, bufferSize);
            if (result == -1) return new byte[0];
            return bytes;
        }
        #endregion

        #region IArrayString
        public string[] BaseInfo()
        {
            string[] data = new string[8];

            data[0] = "ID";
            data[1] = "Nom";
            data[2] = "Role";
            data[3] = "Vie maximum";
            data[4] = "Vie courante";
            data[5] = "Nombre de carte";
            data[6] = "Portée";
            data[7] = "Est vivant ?";

            return data;
        }

        public string[] ToArrayString(bool hideInformation = true)
        {
            string[] data = new string[8];

            data[0] = ID.ToString();
            data[1] = character.name;
            data[2] = hideInformation?(isRoleVisible ? role.ToString() : "?????") : role.ToString();
            data[3] = character.maxLife.ToString();
            data[4] = character.currentLife.ToString();
            data[5] = cards.Count.ToString();
            data[6] = portee.ToString();
            data[7] = isAlive ? "Vivant" : "Mort";

            return data;
        }
        #endregion
    }

    public static class ClientExtend
    {
        /// <summary>
        /// Return true if the ID is in the deck
        /// </summary>
        /// <param name="id">Id to search in deck</param>
        /// <returns></returns>
        public static bool Contains(this Deck<Client> d, int id)
        {
            foreach(Client c in d)
            {
                if(c.ID == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
