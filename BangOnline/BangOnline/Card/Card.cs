﻿namespace BangOnline.Cards
{
    public class Card
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

        public override string ToString()
        {
            return "Nom : " + nom + "\nCouleur : " + couleur.ToString() + "\nValeur : " + value + "\nCible : " + cible.ToString() + "\nDescription : " + desc;
        }
    }
}
