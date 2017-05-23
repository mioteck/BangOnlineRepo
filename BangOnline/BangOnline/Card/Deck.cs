using System;
using System.Collections.Generic;

namespace BangOnline.Cards
{
    public class Deck<T> : List<T>
    {
        public void Shuffle()
        {
            int length = this.Count;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    int index = j;

                    while (j == index)
                    {
                        Random rand = new Random();
                        index = rand.Next(0, length);
                    }

                    this.Switch(j, index);
                }
            }
        }

        public void Switch(int i1, int i2)
        {
            T tmp = this[i1];
            this[i1] = this[i2];
            this[i2] = tmp;
        }
    }
}
