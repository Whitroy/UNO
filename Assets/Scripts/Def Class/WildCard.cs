namespace UNO
{
    public class WildCard : Card
    {
        public WildCard(CardType type) : base(CardGroup.Wild, CardColor.None, type)
        {

        }

        public override void ability()
        {
            if (Type == CardType.DrawFour)
            {
                MatchManager.Instance.AddDrawFour();
            }
            else
            {
                MatchManager.Instance.WildCard();
            }
        }
    }
}