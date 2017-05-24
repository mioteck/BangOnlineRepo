using BangOnline.Common;

namespace BangOnline.Cards
{
    public class Jail : Equipment
    {
        public Jail(string n, Couleur c, Value v, Cible cc, string d) : base(n, c, v, cc, d) { }


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
            data[4] = cible.ToString();
            // data[5] = desc;

            return data;
        }
    }
}