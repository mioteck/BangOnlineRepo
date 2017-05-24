using BangOnline.Common;

namespace BangOnline.Cards
{
    public class Effect : Card
    {
        public int portee;

        public Effect(string n, Couleur c, Value v, Cible cc, string d, int p) : base(n, c, v, cc, d)
        {
            portee = p;
        }

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