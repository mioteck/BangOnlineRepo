﻿using BangOnline.Cards;
using BangOnline.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BangOnline.Common
{
    public class GameState
    {
        public static GameState instance;

        public Deck<Client> clients;

        public Deck<Card> cardPicker;

        public Deck<Card> discardCard;

        public int numberOfPlayers
        {
            get
            {
                return clients.Count;
            }
        }

        public int IndexSherif
        {
            get
            {
                return _indexSherif;
            }
        }

        public int Turn
        {
            get
            {
                return _turn;
            }
        }

        public Client currentPlayer
        {
            get
            {
                return clients[_indexPlayerTurn];
            }
        }

        private int _indexSherif = -1;

        private int _turn = 0;

        private int _indexPlayerTurn = 0;

        public GameState(Deck<Client> c, Deck<Card> cc)
        {
            clients = c;
            cardPicker = cc;
            discardCard = new Deck<Card>();

            for(int i=0; i<clients.Count; i++)
            {
                if (clients[i].role == Role.Sherif)
                {
                    _indexSherif = i;
                    break;
                }
            }

            instance = this;
        }

        public void Draw(int id, int numberOfCard)
        {
            if(cardPicker.Count < numberOfCard)
            {
                discardCard.Shuffle();
                cardPicker.AddRange(discardCard);
                discardCard.Clear();
            }
            Client client = clients[id];
            client.SetCards(cardPicker.PopFirstElement(numberOfCard));
        }

        public void DiscardCard(int id, int indexCard = -1, int indexEquipment = -1)
        {
            Client target = clients[id];
            if(indexCard != -1)
            {
                discardCard.Add(target.cards.PopElement(indexCard));
            }
            if(indexEquipment != -1)
            {
                discardCard.Add(target.equipments.PopElement(indexEquipment));
            }
        }

        public int NextPlayer()
        {
            if(_turn == 0)
            {
                _indexPlayerTurn = _indexSherif;
            }
            else
            {
                _indexPlayerTurn++;
                if(_indexPlayerTurn >= numberOfPlayers)
                {
                    _indexPlayerTurn = 0;
                }
            }
            if(_indexPlayerTurn == _indexSherif)
            {
                _turn++;
            }
            return _indexPlayerTurn;
        }

        public Role CheckEndGame()
        {
            Client sherif = clients[_indexSherif];

            IEnumerable<Client> adjudants = clients.Where(x => x.role == Role.Adjudant);

            IEnumerable<Client> renegats = clients.Where(x => x.role == Role.Renegat);

            IEnumerable<Client> horslaloi = clients.Where(x => x.role == Role.HorsLaLoi);

            // Victoire sherif
            bool isVictorySherif = true;
            foreach(Client c in renegats)
            {
                if(c.isAlive)
                {
                    isVictorySherif = false;
                }
            }
            foreach(Client c in horslaloi)
            {
                if(c.isAlive)
                {
                    isVictorySherif = false;
                }
            }
            if(isVictorySherif && sherif.isAlive)
            {
                return Role.Sherif;
            }

            // Victoire Renegat
            bool isVictoryRenegat = true;
            if(sherif.isAlive)
            {
                isVictoryRenegat = false;
            }
            foreach(Client c in adjudants)
            {
                if(c.isAlive)
                {
                    isVictoryRenegat = false;
                }
            }
            foreach(Client c in horslaloi)
            {
                if(c.isAlive)
                {
                    isVictoryRenegat = false;
                }
            }
            if(isVictoryRenegat && renegats.Where(x => x.isAlive == true).Count() == 1)
            {
                return Role.Renegat;
            }

            // Victoire Hors la loi
            if(!sherif.isAlive)
            {
                return Role.HorsLaLoi;
            }

            return Role.None;
        }

        public bool GainHP(int index)
        {
            Character c = clients[index].character;   
            if(c.currentLife >= c.maxLife)
            {
                return false;
            }
            c.currentLife++;
            return true;
        }

        public bool LooseHP(int index)
        {
            if(clients[index].isAlive)
            {
                clients[index].character.currentLife--;
                return true;
            }
            return false;
        }

        public void PlayCard(int id, int indexCard, int indexTarget, string targetCard = "")
        {
            Deck<Card> cards = clients[id].cards;
            bool isOK;
            if(indexTarget == -1)
            {
                isOK = cards[indexCard].Run(id);
            }
            else if(indexTarget != -1)
            {
                isOK = cards[indexCard].Run(new object[] { id, indexTarget });
            }
            else
            {
                isOK = cards[indexCard].Run(new object[] { id, indexTarget, targetCard });
            }
            if(isOK)
            {
                DiscardCard(id, indexCard);
            }
        }

        public int Distance(Client a, Client b)
        {
            int aid, bid;
            if(a.ID < b.ID)
            {
                aid = a.ID;
                bid = b.ID;
            }
            else
            {
                aid = b.ID;
                bid = a.ID;
            }
            return Math.Min(bid - aid, clients.Count - bid + aid);

        }

        #region Infos
        public string GetPlayersInfo()
        {
            return IFormatter.Formating(clients);
        }

        public string GetPlayerInfo(int idRequest, int id)
        {
            Deck<Client> client = new Deck<Client>();
            client.Add(clients[id]);
            return IFormatter.Formating(client, idRequest != id);
        }

        public string GetCardDescription(int id, int index)
        {
            return clients[id].cards[index].GetDescription();
        }

        public string GetCards(int id)
        {
            Deck<Card> cards = clients[id].cards;
            if (cards.Count == 0)
                return "Vous ne possedez aucune carte !";
            return IFormatter.Formating(cards);
        }

        public List<int> GetIndexByRole(Role r) // int or list<int>
        {
            List<int> indexes = new List<int>();
            foreach(Client c in clients)
            {
                if (c.role == r)
                    indexes.Add(c.ID);
            }
            return indexes;
        }
        #endregion
    }
}
