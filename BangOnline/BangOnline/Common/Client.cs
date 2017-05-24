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
        public List<Card> cards;
        /// <summary>
        /// The list of the equipment equiped
        /// </summary>
        public List<Equipment> equipments;
        /// <summary>
        /// The shooting range
        /// </summary>
        public int portee;
        /// <summary>
        /// Show if the player is alive or not
        /// </summary>
        public bool isAlive;

        static int id = 0;

        public Client(TcpClient c)
        {
            tcpClient = c;
            stream = tcpClient.GetStream();

            IPEndPoint ipClient = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            ipAddr = ipClient.Address.ToString();

            role = Role.None;

            cards = new List<Card>();
            equipments = new List<Equipment>();

            isAlive = true;

            ID = id;
            id++;

            portee = 1;
        }

        public Client() // For test
        {
            role = Role.None;

            cards = new List<Card>();
            equipments = new List<Equipment>();

            ID = id;
            id++;

            isAlive = true;

            portee = 1;
        }

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

        public string[] BaseInfo()
        {
            string[] data = new string[7];

            data[0] = "ID";
            data[1] = "Nom";
            data[2] = "Vie maximum";
            data[3] = "Vie courante";
            data[4] = "Nombre de carte";
            data[5] = "Portée";
            data[6] = "Est vivant ?";

            return data;
        }

        public string[] ToArrayString()
        {
            string[] data = new string[7];

            data[0] = ID.ToString();
            data[1] = character.name;
            data[2] = character.maxLife.ToString();
            data[3] = character.currentLife.ToString();
            data[4] = cards.Count.ToString();
            data[5] = portee.ToString();
            data[6] = isAlive ? "Vivant" : "Mort";

            return data;
        }
    }
}
