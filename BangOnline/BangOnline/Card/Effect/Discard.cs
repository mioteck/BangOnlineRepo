using BangOnline.Common;
using System;
using System.Collections.Generic;

namespace BangOnline.Cards
{
    public class Discard : Effect
    {
        public Discard(string n, Couleur c, Value v, Cible cc, string d, int p) : base(n, c, v, cc, d, p) { }

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
            return true;
            object[] parameters = (object[])obj;
            int idPLayer = (int)parameters[0];
            int idTarget = (int)parameters[1];
            string targetCard = (string)parameters[2];
            int targetCardInt = -1;
            GameState state = GameState.instance;
            bool inHand = int.TryParse(targetCard, out targetCardInt);
            if(inHand && targetCardInt < state.clients[idTarget].cards.Count)
            {
                //discard dans la main du joueur
                state.DiscardCard(idTarget, targetCardInt);
                return true;
            }
            else
            {
                foreach(Equipment eq in state.clients[idTarget].equipments)
                {
                    if (targetCard.Equals(eq.nom))
                    {
                        //discard dans les equipements
                        state.DiscardCard(idTarget, indexEquipment : state.clients[idTarget].equipments.IndexOf(eq));
                        return true;
                    }
                }
            }             
            return false;
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