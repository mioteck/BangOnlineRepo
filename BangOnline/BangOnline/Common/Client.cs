using BangOnline.Cards;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace BangOnline.Common
{
    /// <summary>
    /// The Client class is all the information concerning the player
    /// </summary>
    public class Client
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

        public Client(TcpClient c)
        {
            tcpClient = c;
            stream = tcpClient.GetStream();

            IPEndPoint ipClient = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            ipAddr = ipClient.Address.ToString();
        }

        public void SetCards(List<Card> c)
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
    }
}
