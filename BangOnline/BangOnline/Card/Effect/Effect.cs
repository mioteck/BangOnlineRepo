namespace BangOnline.Cards
{
    public class Effect : Card
    {
        public int portee;

        public Effect(string n, Couleur c, Value v, Cible cc, string d, int p) : base(n, c, v, cc, d)
        {
            portee = p;
        }

        public override string ToString()
        {
            return "Nom : " + nom + "\nCouleur : " + couleur.ToString() + "\nValeur : " + value + "\nCible : " + cible.ToString() + "\nPortée : " + portee + "\nDescription : " + desc;
        }
    }
}