using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Weapon : Equipment
{
    public Weapon(string n, Couleur c, int v, Cible cc, string d) : base(n, c, v, cc, d) { }
}

