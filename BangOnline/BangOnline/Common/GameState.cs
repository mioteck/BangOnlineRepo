using BangOnline.Cards;
using BangOnline.Common;
using System.Collections.Generic;

namespace BangOnline.Common
{
    public class GameState
    {
        public List<Client> clients;

        public Deck<Card> cardPicker;

        public Deck<Card> discardCard;
    }
}
