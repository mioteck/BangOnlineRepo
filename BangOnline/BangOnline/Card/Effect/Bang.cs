﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Bang : Effect, IRunning
{
    public Bang(string n, Couleur c, int v, Cible cc, string d, int p) : base(n, c, v, cc, d, p) { }

    public void Run()
    {
        throw new NotImplementedException();
    }
}

