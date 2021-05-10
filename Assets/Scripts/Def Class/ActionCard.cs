namespace UNO
{
    public class ActionCard : Card
    {
        public ActionCard(CardColor color, CardType type) : base(CardGroup.Action, color, type)
        {

        }

        public override void ability()
        {
            if(Type == CardType.DrawTwo){
                MatchManager.Instance.AddTwoCards();
            }
            else if(Type == CardType.Reverse)
            {
                MatchManager.Instance.ReverseGame();
            }
            else
            {
                MatchManager.Instance.SkipPlayer();
            }
        }


    }
}