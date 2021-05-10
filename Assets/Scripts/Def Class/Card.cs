using System.Diagnostics;

namespace UNO
{
    public abstract class Card
    {
        private CardGroup group;
        private CardColor color;
        private CardType CardType;
        private bool isSelected;

        public virtual void ability() { }

        protected Card(CardGroup group, CardColor color, CardType type)
        {
            this.group = group;
            this.color = color;
            this.CardType = type;
        }

        public CardGroup Group { get => group; set => group = value; }
        public CardColor Color { get => color; set => color = value; }
        public CardType Type { get => CardType; set => CardType = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }

        public override bool Equals(object obj)
        {
            return obj is Card card &&
                   group == card.group &&
                   color == card.color &&
                   CardType == card.CardType;
        }

        public override int GetHashCode()
        {
            int hashCode = -1788201295;
            hashCode = hashCode * -1521134295 + group.GetHashCode();
            hashCode = hashCode * -1521134295 + color.GetHashCode();
            hashCode = hashCode * -1521134295 + CardType.GetHashCode();
            return hashCode;
        }

        private string name()
        {
            string res ="";
            switch (CardType)
            {
                case CardType.Zero:
                    res += "Zero of Color ";
                    break;
                case CardType.One:
                    res += "One of Color ";
                    break;
                case CardType.Two:
                    res += "Two of Color ";
                    break;
                case CardType.Three:
                    res += "Three of Color ";
                    break;
                case CardType.Four:
                    res += "Four of Color ";
                    break;
                case CardType.Five:
                    res += "Five of Color ";
                    break;
                case CardType.Six:
                    res += "Six of Color ";
                    break;
                case CardType.Seven:
                    res += "Seven of Color ";
                    break;
                case CardType.Eight:
                    res += "Eight of Color ";
                    break;
                case CardType.Nine:
                    res += "Nine of Color ";
                    break;
                case CardType.DrawTwo:
                    res += "Draw Two of Color ";
                    break;
                case CardType.Reverse:
                    res += "Reverse of Color ";
                    break;
                case CardType.Skip:
                    res += "Skip of Color ";
                    break;
                case CardType.DrawFour:
                    res += "Draw Four";
                    break;
                case CardType.WildCard:
                    res += "Wild Card ";
                    break;
            }
            switch (color)
            {
                case CardColor.Red:
                    res += "Red";
                    break;
                case CardColor.Blue:
                    res += "Blue";
                    break;
                case CardColor.Yellow:
                    res += "Yellow";
                    break;
                case CardColor.Green:
                    res += "Green";
                    break;
                case CardColor.None:
                    break;
            }
            return res;
        }

        public override string ToString()
        {
            return name();
        }
    }
}