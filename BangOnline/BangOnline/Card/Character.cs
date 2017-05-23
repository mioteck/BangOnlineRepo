using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BangOnline.Cards
{
    /// <summary>
    /// The Character class contain the name, the max life and the current life of the character
    /// </summary>
    public class Character
    {
        /// <summary>
        /// The name of the character
        /// </summary>
        public string name;
        /// <summary>
        /// The max life of the character
        /// </summary>
        public int maxLife;
        /// <summary>
        /// The current life of the character
        /// </summary>
        public int currentLife;

        public void SetToMaxLife()
        {
            currentLife = maxLife;
        }
    }
}
