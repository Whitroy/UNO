using System.Collections.Generic;
using System;

namespace UNO{
    public class Deck
    {
        private List<Card> cardDeck;
        private int top;
        private int deckSize;

        public List<Card> CardDeck { get => cardDeck; }

        public Deck()
        {
            cardDeck = new List<Card>();
            top = 0;
            deckSize = 108;
            IntializeDeck();
            Shuffle();
        }

        private void IntializeDeck()
        {
            int i = 0, j = 0, k = 0;
            while (i < deckSize)
            {
                if (i < 4)
                {
                    CardDeck.Add(new NumberCard(CardColor.Red + j%4, CardType.Zero));
                    j++;
                }
                else if (i < 76)
                {
                    CardDeck.Add(new NumberCard(CardColor.Red+j%4, CardType.One + k%9));
                    k++;
                    if(k%9 == 0)
                    {
                        j++;
                    }
                }
                else if (i < 96)
                {
                    CardDeck.Add(new ActionCard(CardColor.Red + j % 4, CardType.DrawTwo));
                    i++;
                    CardDeck.Add(new ActionCard(CardColor.Red + j % 4, CardType.Skip));
                    i++;
                    CardDeck.Add(new ActionCard(CardColor.Red + j % 4, CardType.Reverse));
                    j++;
                }
                else
                {
                    CardDeck.Add(new WildCard(CardType.DrawFour));
                    i++;
                    CardDeck.Add(new WildCard(CardType.WildCard));
                }
                i++;
            }
        }

        public void reset(int num = -1)
        {
            top = 0;
        }

        private void Shuffle()
        {
            Random random = new Random();
            for(int i = 0; i < deckSize; i++)
            {
                Swap(i, random.Next() % deckSize);
            }
        }

        private void Swap(int a, int b)
        {
            Card temp = CardDeck[a];
            CardDeck[a] = CardDeck[b];
            CardDeck[b] = temp;
        }

        public Card GetTopCard()
        {
            if (isEmpty())
                return null;
            if (top > deckSize)
                reset();
            return CardDeck[top++];
        }

        public void GetBackCard()
        {
            Swap(top - 1, deckSize - 1);
            top--;
        }
        public bool isEmpty()
        {
            if (cardDeck.Count == 0)
                return true;
            return false;
        }

        public void Distribute(int numberOfCards,List<IPlayer> players)
        {
            int size = players.Count;
            for(int i = 0; i < numberOfCards * size; i++)
            {
                PlayerHelper.AddCard(GetTopCard(), players[i % size]);
                //players[i % size].AddCard(GetTopCard());
            }
        }
    }
}
