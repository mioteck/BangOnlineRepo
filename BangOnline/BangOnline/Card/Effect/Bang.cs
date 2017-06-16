using BangOnline.Common;
using System;
using System.Collections.Generic;

namespace BangOnline.Cards
{
    public class Bang : Effect
    {
        public Bang(string n, Couleur c, Value v, Cible cc, string d, int p) : base(n, c, v, cc, d, p) { }


        public string[] BaseInfo()
        {
            string[] data = new string[5];

            data[0] = "Nom";
            data[1] = "Couleur";
            data[2] = "Valeur";
            data[3] = "Cible";
            data[4] = "Portée";
            //data[5] = "Description";

            return data;
        }

        public override bool Run(object obj)
        {
            object[] parameters = (object[])obj;
            int playerID = (int)parameters[0];
            int targetID = (int)parameters[1];

            GameState state = GameState.instance;
            Client player = state.clients[playerID];
            int porteeP = player.portee;

            Client target = state.clients[targetID];

            if (state.Distance(player, target) > porteeP)
                return false;

            foreach(Card c in target.cards)
            {
                if(c.nom == "Miss")
                {
                    state.DiscardCard(target.ID, target.cards.IndexOf(c));
                    return false;
                }
            }
            state.LooseHP(target.ID);
            return true;
        }

        public string[] ToArrayString(bool hideInformation = true)
        {
            string[] data = new string[5];

            data[0] = nom;
            data[1] = couleur.ToString();
            data[2] = value.ToString();
            data[3] = cible.ToString();
            data[4] = portee.ToString();
            //data[5] = desc;

            return data;
        }
    }

}