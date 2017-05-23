namespace BangOnline.Cards
{
    public class Effect : Card
    {
        public int porte;

        public Effect(string n, Couleur c, int v, Cible cc, string d, int p) : base(n, c, v, cc, d)
        {
            porte = p;
        }

        public override string ToString()
        {
            return "Nom : " + nom + "\nCouleur : " + couleur.ToString() + "\nValeur : " + value + "\nCible : " + cible.ToString() + "\nPortée : " + porte + "\nDescription : " + desc;
        }
    }
}