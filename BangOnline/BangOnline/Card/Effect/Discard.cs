using System;

namespace BangOnline.Cards
{
    public class Discard : Effect, IRunning
    {
        public Discard(string n, Couleur c, int v, Cible cc, string d, int p) : base(n, c, v, cc, d, p) { }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}