﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Discard : Effect, IRunning
{
    public Discard(string n, Couleur c, int v, Cible cc, string d, int p) : base(n, c, v, cc, d, p) { }

    public void Run()
    {
        throw new NotImplementedException();
    }
}