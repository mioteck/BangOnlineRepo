using BangOnline.Common;

namespace BangOnline.Cards
{
    public class Card: IArrayString
    {
        public string nom;
        public Couleur couleur;
        public Value value;
        public Cible cible;
        public string desc;

        public Card(string n, Couleur c, Value v, Cible cc, string d)
        {
            nom = n;
            couleur = c;
            value = v;
            cible = cc;
            desc = d;
        }

        public string[] BaseInfo()
        {
            string[] data = new string[4];

            data[0] = "Nom";
            data[1] = "Couleur";
            data[2] = "Valeur";
            data[3] = "Cible";
            //data[4] = "Description";

            return data;
        }

        public string[] ToArrayString(bool hideInformation = true)
        {
            string[] data = new string[4];

            data[0] = nom;
            data[1] = couleur.ToString();
            data[2] = value.ToString();
            data[3] = cible.ToString();
            //data[4] = desc;

            return data;
        }
    }
}
